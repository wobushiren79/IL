-- Reusable Aseprite CLI operations. All source edits use saveCopyAs by default.

local pc = app.pixelColor

local function param(name, default)
  local value = app.params[name]
  if value == nil or value == "" then
    return default
  end
  return value
end

local function boolParam(name, default)
  local value = param(name, nil)
  if value == nil then
    return default
  end
  value = string.lower(value)
  return value == "1" or value == "true" or value == "yes"
end

local function intParam(name, default)
  local value = param(name, nil)
  if value == nil then
    return default
  end
  local number = tonumber(value)
  assert(number ~= nil and number == math.floor(number), name .. " must be an integer")
  return number
end

local controlEscapes = {
  ["\b"] = "\\b",
  ["\f"] = "\\f",
  ["\n"] = "\\n",
  ["\r"] = "\\r",
  ["\t"] = "\\t"
}

local function jsonString(value)
  local text = value:gsub("\\", "\\\\")
  text = text:gsub('"', '\\"')
  text = text:gsub("[%z\1-\31\127]", function(character)
    return controlEscapes[character] or string.format("\\u%04x", string.byte(character))
  end)
  return '"' .. text .. '"'
end

local function jsonEncode(value)
  local valueType = type(value)
  if valueType == "nil" then
    return "null"
  elseif valueType == "string" then
    return jsonString(value)
  elseif valueType == "number" or valueType == "boolean" then
    return tostring(value)
  elseif valueType ~= "table" then
    error("Unsupported JSON value type: " .. valueType)
  end

  local count = 0
  local maxIndex = 0
  local isArray = true
  for key, _ in pairs(value) do
    count = count + 1
    if type(key) ~= "number" or key < 1 or key ~= math.floor(key) then
      isArray = false
      break
    end
    if key > maxIndex then
      maxIndex = key
    end
  end

  if count == 0 then
    return "[]"
  end

  local parts = {}
  if isArray and count == maxIndex then
    for index = 1, maxIndex do
      parts[#parts + 1] = jsonEncode(value[index])
    end
    return "[" .. table.concat(parts, ",") .. "]"
  end

  for key, item in pairs(value) do
    parts[#parts + 1] = jsonString(tostring(key)) .. ":" .. jsonEncode(item)
  end
  return "{" .. table.concat(parts, ",") .. "}"
end

local function writeJson(path, value)
  if path == nil then
    return
  end
  local file, openError = io.open(path, "wb")
  assert(file ~= nil, "Cannot write report: " .. tostring(openError))
  file:write(jsonEncode(value))
  file:close()
end

local function normalizePath(path)
  local normalized = path:gsub("\\", "/"):gsub("/+$", "")
  return string.lower(normalized)
end

local function checkOutput(path, sourcePath)
  assert(path ~= nil, "out is required")
  assert(not app.fs.isDirectory(path), "out points to a directory")
  if sourcePath ~= nil then
    assert(normalizePath(path) ~= normalizePath(sourcePath), "Source and output must differ")
  end
  if app.fs.isFile(path) then
    assert(boolParam("overwrite", false), "Output exists; pass overwrite=true to replace it")
  end
  local parent = app.fs.filePath(path)
  assert(parent ~= "" and app.fs.isDirectory(parent), "Output directory does not exist")
end

local function openSource()
  local sourcePath = param("src", nil)
  assert(sourcePath ~= nil, "src is required")
  local sprite = app.open(sourcePath)
  assert(sprite ~= nil, "Cannot open source: " .. sourcePath)
  return sprite, sourcePath
end

local function assertRgb(sprite)
  assert(sprite.colorMode == ColorMode.RGB, "This operation currently supports RGB/RGBA sprites only")
end

local function colorModeName(sprite)
  if sprite.colorMode == ColorMode.RGB then
    return "rgb"
  elseif sprite.colorMode == ColorMode.INDEXED then
    return "indexed"
  elseif sprite.colorMode == ColorMode.GRAYSCALE then
    return "grayscale"
  end
  return tostring(sprite.colorMode)
end

local function parseColor(text, name)
  assert(text ~= nil, name .. " is required")
  local hex = text:gsub("^#", "")
  if #hex == 6 then
    hex = hex .. "FF"
  end
  assert(#hex == 8 and hex:match("^[0-9a-fA-F]+$") ~= nil, name .. " must be RRGGBB or RRGGBBAA")
  local red = tonumber(hex:sub(1, 2), 16)
  local green = tonumber(hex:sub(3, 4), 16)
  local blue = tonumber(hex:sub(5, 6), 16)
  local alpha = tonumber(hex:sub(7, 8), 16)
  return pc.rgba(red, green, blue, alpha)
end

local function parseFrameFilter()
  local text = param("frames", nil)
  if text == nil then
    return nil
  end
  local frames = {}
  for item in text:gmatch("[^,]+") do
    local frameNumber = tonumber(item)
    assert(frameNumber ~= nil and frameNumber >= 1 and frameNumber == math.floor(frameNumber), "Invalid frames list")
    frames[frameNumber] = true
  end
  return frames
end

local function inspectSprite()
  local sprite, sourcePath = openSource()
  local layers = {}
  for _, layer in ipairs(sprite.layers) do
    layers[#layers + 1] = {
      name = layer.name,
      is_group = layer.isGroup,
      is_visible = layer.isVisible,
      opacity = layer.opacity
    }
  end

  local frames = {}
  for index, frame in ipairs(sprite.frames) do
    frames[#frames + 1] = { frame = index, duration = frame.duration }
  end

  local cels = {}
  for _, cel in ipairs(sprite.cels) do
    cels[#cels + 1] = {
      frame = cel.frameNumber,
      layer = cel.layer.name,
      x = cel.position.x,
      y = cel.position.y,
      width = cel.image.width,
      height = cel.image.height
    }
  end

  local slices = {}
  if sprite.slices ~= nil then
    for _, slice in ipairs(sprite.slices) do
      slices[#slices + 1] = { name = slice.name }
    end
  end

  local report = {
    operation = "inspect",
    source = sourcePath,
    width = sprite.width,
    height = sprite.height,
    color_mode = colorModeName(sprite),
    layer_count = #sprite.layers,
    frame_count = #sprite.frames,
    cel_count = #sprite.cels,
    layers = layers,
    frames = frames,
    cels = cels,
    slices = slices
  }
  sprite:close()

  local reportPath = param("report", nil)
  if reportPath == nil then
    print(jsonEncode(report))
  else
    writeJson(reportPath, report)
  end
end

local function createSprite()
  local outputPath = param("out", nil)
  checkOutput(outputPath, nil)
  local width = intParam("width", nil)
  local height = intParam("height", nil)
  assert(width ~= nil and width > 0, "width must be a positive integer")
  assert(height ~= nil and height > 0, "height must be a positive integer")

  local sprite = Sprite(width, height, ColorMode.RGB)
  sprite.layers[1].name = param("layer", "Layer 1")
  for pixel in sprite.cels[1].image:pixels() do
    pixel(pc.rgba(0, 0, 0, 0))
  end
  sprite:saveAs(outputPath)
  sprite:close()

  writeJson(param("report", nil), {
    operation = "create",
    output = outputPath,
    width = width,
    height = height,
    color_mode = "rgb"
  })
end

local function recolorSprite()
  local sprite, sourcePath = openSource()
  assertRgb(sprite)
  local outputPath = param("out", nil)
  checkOutput(outputPath, sourcePath)

  local fromColor = parseColor(param("from", nil), "from")
  local toColor = parseColor(param("to", nil), "to")
  local layerName = param("layer", nil)
  local frameFilter = parseFrameFilter()
  local includeHidden = boolParam("include_hidden", false)
  local changedPixels = 0
  local changedCels = 0

  app.transaction("Exact recolor", function()
    for _, cel in ipairs(sprite.cels) do
      local layerMatches = layerName == nil or cel.layer.name == layerName
      local frameMatches = frameFilter == nil or frameFilter[cel.frameNumber] == true
      local visibilityMatches = includeHidden or cel.layer.isVisible
      if layerMatches and frameMatches and visibilityMatches then
        local celChanges = 0
        for pixel in cel.image:pixels() do
          if pixel() == fromColor then
            pixel(toColor)
            celChanges = celChanges + 1
          end
        end
        if celChanges > 0 then
          changedPixels = changedPixels + celChanges
          changedCels = changedCels + 1
        end
      end
    end
  end)

  assert(changedPixels > 0 or boolParam("allow_no_match", false), "No pixels matched the requested color and scope")
  sprite:saveCopyAs(outputPath)
  sprite:close()

  writeJson(param("report", nil), {
    operation = "recolor",
    source = sourcePath,
    output = outputPath,
    changed_pixels = changedPixels,
    changed_cels = changedCels
  })
end

local function addPngLayer()
  local sprite, sourcePath = openSource()
  assertRgb(sprite)
  local outputPath = param("out", nil)
  checkOutput(outputPath, sourcePath)

  local pngPath = param("png", nil)
  assert(pngPath ~= nil and app.fs.isFile(pngPath), "png must point to an existing image")
  local layerName = param("layer", nil)
  assert(layerName ~= nil, "layer is required")
  for _, layer in ipairs(sprite.layers) do
    assert(layer.name ~= layerName, "A top-level layer with this name already exists")
  end

  local frameIndex = intParam("frame", 1)
  assert(frameIndex >= 1 and frameIndex <= #sprite.frames, "frame is outside the sprite")
  local afterName = param("after", nil)
  local referenceLayer = nil
  if afterName ~= nil then
    for _, layer in ipairs(sprite.layers) do
      if layer.name == afterName then
        referenceLayer = layer
        break
      end
    end
    assert(referenceLayer ~= nil, "Reference layer not found: " .. afterName)
  end
  local x = intParam("x", 0)
  local y = intParam("y", 0)
  local pngSprite = app.open(pngPath)
  assert(pngSprite ~= nil, "Cannot open PNG: " .. pngPath)
  assertRgb(pngSprite)
  local image = pngSprite.cels[1].image:clone()
  local imageWidth = image.width
  local imageHeight = image.height
  pngSprite:close()
  app.activeSprite = sprite

  app.transaction("Add PNG layer", function()
    local layer = sprite:newLayer()
    layer.name = layerName
    if referenceLayer ~= nil then
      layer.stackIndex = referenceLayer.stackIndex + 1
    end
    sprite:newCel(layer, sprite.frames[frameIndex], image, Point(x, y))
  end)

  sprite:saveCopyAs(outputPath)
  sprite:close()

  writeJson(param("report", nil), {
    operation = "add_layer",
    source = sourcePath,
    output = outputPath,
    layer = layerName,
    after = afterName,
    frame = frameIndex,
    x = x,
    y = y,
    width = imageWidth,
    height = imageHeight
  })
end

local function setCel()
  local sprite, sourcePath = openSource()
  assertRgb(sprite)
  local outputPath = param("out", nil)
  checkOutput(outputPath, sourcePath)

  local layerName = param("layer", nil)
  assert(layerName ~= nil, "layer is required")
  local targetLayer = nil
  for _, layer in ipairs(sprite.layers) do
    if layer.name == layerName then
      targetLayer = layer
      break
    end
  end
  assert(targetLayer ~= nil, "Layer not found: " .. layerName)

  local frameIndex = intParam("frame", 1)
  assert(frameIndex >= 1 and frameIndex <= #sprite.frames, "frame is outside the sprite")
  local existingCel = targetLayer:cel(frameIndex)
  local x = intParam("x", existingCel ~= nil and existingCel.position.x or 0)
  local y = intParam("y", existingCel ~= nil and existingCel.position.y or 0)
  local pngPath = param("png", nil)
  assert(pngPath ~= nil and app.fs.isFile(pngPath), "png must point to an existing image")

  local pngSprite = app.open(pngPath)
  assert(pngSprite ~= nil, "Cannot open PNG: " .. pngPath)
  assertRgb(pngSprite)
  local image = pngSprite.cels[1].image:clone()
  local imageWidth = image.width
  local imageHeight = image.height
  pngSprite:close()
  app.activeSprite = sprite

  app.transaction("Set cel image", function()
    if existingCel ~= nil then
      existingCel.image = image
      existingCel.position = Point(x, y)
    else
      sprite:newCel(targetLayer, sprite.frames[frameIndex], image, Point(x, y))
    end
  end)

  sprite:saveCopyAs(outputPath)
  sprite:close()

  writeJson(param("report", nil), {
    operation = "set_cel",
    source = sourcePath,
    output = outputPath,
    layer = layerName,
    frame = frameIndex,
    replaced_existing = existingCel ~= nil,
    x = x,
    y = y,
    width = imageWidth,
    height = imageHeight
  })
end

local operation = param("op", "inspect")
if operation == "inspect" then
  inspectSprite()
elseif operation == "create" then
  createSprite()
elseif operation == "recolor" then
  recolorSprite()
elseif operation == "add_layer" then
  addPngLayer()
elseif operation == "set_cel" then
  setCel()
else
  error("Unsupported op: " .. operation .. ". Expected inspect, create, recolor, add_layer, or set_cel")
end

print("ASEPRITE_TOOL_OK")

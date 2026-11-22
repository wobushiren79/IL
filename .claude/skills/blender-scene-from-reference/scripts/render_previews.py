"""Render the saved scene from named cameras into a new preview directory."""

import argparse
import re
import sys
from pathlib import Path

import bpy


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--blend", required=True, type=Path)
    parser.add_argument("--output", required=True, type=Path)
    parser.add_argument("--camera", action="append", help="Exact scene camera name. May be repeated.")
    parser.add_argument("--scale", type=int, choices=range(1, 101), default=100)
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    args = parser.parse_args(argv)

    bpy.ops.wm.open_mainfile(filepath=str(args.blend.resolve(strict=True)))
    scene = bpy.context.scene
    names = args.camera or ([scene.camera.name] if scene.camera else [])
    if not names:
        parser.error("The saved scene has no active camera. Specify --camera.")
    output = args.output.resolve()
    shots = {}
    for name in names:
        camera = scene.objects.get(name)
        if camera is None or camera.type != "CAMERA":
            parser.error(f"No camera in the active scene named {name!r}.")
        filename = re.sub(r"[^\w.-]+", "_", name).strip(".") + ".png"
        destination = output / filename
        if destination in shots or destination.exists():
            parser.error(f"Preview path already exists or is repeated: {destination}")
        shots[destination] = camera

    output.mkdir(parents=True, exist_ok=True)
    scene.render.resolution_percentage = args.scale
    scene.render.image_settings.file_format = "PNG"
    scene.render.use_file_extension = True
    for destination, camera in shots.items():
        scene.camera = camera
        scene.render.filepath = str(destination)
        bpy.ops.render.render(write_still=True)


if __name__ == "__main__":
    main()

---
name: system-network
description: 网络请求系统开发：WebRequest、UnityWebRequest、网络回调接口、图片/纹理下载。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Web/
  - Assets/FrameWork/Scripts/Utils/LoadWWWUtil.cs
---

# 网络请求系统 (Network System) 开发代理

你负责 [FrameWork/Scripts/Web/](Assets/FrameWork/Scripts/Web/) 中网络请求系统的开发。

## 职责范围

### 网络请求
- **WebRequest** - 网络请求类 [Assets/FrameWork/Scripts/Web/WebRequest.cs](Assets/FrameWork/Scripts/Web/WebRequest.cs)

### 回调接口
- **IWebRequestCallBack** - 通用请求回调
- **IWebRequestForSpriteCallBack** - Sprite 请求回调
- **IWebRequestForTextureCallBack** - Texture 请求回调

### 网络加载
- **LoadWWWUtil** - 网络资源加载工具 [Assets/FrameWork/Scripts/Utils/LoadWWWUtil.cs](Assets/FrameWork/Scripts/Utils/LoadWWWUtil.cs)

## 约束

- 网络请求使用 UnityWebRequest
- 回调接口与 WebRequest 一同维护在 `Assets/FrameWork/Scripts/Web/`
- 网络资源加载后需缓存，避免重复请求
- 网络错误需有超时和重试机制
- 不在主线程等待网络响应，使用协程或 async

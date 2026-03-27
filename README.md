# Unity Camera Control System

<img width="1920" height="872" alt="image" src="https://github.com/user-attachments/assets/3f60d2c6-a2cb-482d-b260-bf96d3c9f53d" />

![ezgif-2519c8c0b19439bf](https://github.com/user-attachments/assets/cb0558fb-a367-4db0-ac81-ff03ae7167b6)
![ezgif-27e9d839fb7159b5](https://github.com/user-attachments/assets/b4c56e70-8de3-4608-9eac-f6289483bb62)


---


## 项目简介

本项目是一个基于 Unity 实现的通用相机控制系统，结合 **Cinemachine** 与 **Unity Input System**，实现了适用于多种游戏类型的相机交互功能。

系统同时支持 **3D 相机控制** 与 **2D 相机控制** 两种模式，适用于 RTS、模拟经营、俯视角探索及地图类项目，具备良好的复用性与扩展性。

---

## 功能特性

### 3D 相机控制（CameraSystem）
- WASD 键控制相机平移
- Q / E 控制相机旋转
- 鼠标滚轮控制缩放（远近变化）
- 鼠标右键拖拽平移视角
- 屏幕边缘滚动（Edge Scrolling）

### 2D 相机控制（CameraSystem2D）
- WASD 键控制平移
- Q / E 控制旋转
- 鼠标滚轮控制正交缩放
- 鼠标右键拖拽移动
- 屏幕边缘滚动

---

## 技术实现

### 1. 输入系统（Input System）
使用 Unity 新输入系统统一管理玩家输入：
- 键盘移动（WASD）
- 鼠标输入（滚轮、拖拽）
- 按键控制（旋转）

---

### 2. 相机控制（Cinemachine）

#### 3D 相机
- 使用 `CinemachineVirtualCamera`
- 通过 `CinemachineTransposer.m_FollowOffset` 控制：
  - 相机高度
  - 与目标距离
  - 缩放效果

#### 2D 相机
- 使用正交相机（Orthographic Camera）
- 通过 `OrthographicSize` 控制缩放

---

### 3. 拖拽与边缘滚动实现

#### 鼠标拖拽
- 记录鼠标按下位置
- 计算偏移量
- 转换为世界坐标移动相机

#### 边缘滚动
- 判断鼠标是否接近屏幕边缘
- 按方向持续移动相机

---

### 4. 参数配置（可扩展性）

通过 `SerializeField` 暴露关键参数：

- 移动速度（Move Speed）
- 旋转速度（Rotation Speed）
- 缩放范围（Zoom Range）
- 是否启用边缘滚动
- 是否启用拖拽移动

支持在 Inspector 面板快速调参，方便不同项目复用。

---

## 项目结构
Scripts/
├── CameraSystem.cs // 3D 相机控制
├── CameraSystem2D.cs // 2D 相机控制

---

## 使用方式

1. 导入项目到 Unity
2. 安装 Cinemachine（Package Manager）
3. 创建 Virtual Camera
4. 挂载对应脚本：
   - 3D 使用 `CameraSystem`
   - 2D 使用 `CameraSystem2D`
5. 在 Inspector 中配置参数
6. 运行即可使用

---

## 项目亮点

- 同时支持 **2D / 3D 相机系统**
- 基于 **Cinemachine 实现工业级相机控制**
- 输入系统与相机逻辑解耦
- 支持多种常见交互方式（拖拽 / 边缘滚动 / 缩放）
- 可复用模块设计，适用于多个项目

---

## 应用场景

- RTS（即时战略）
- 模拟经营类游戏
- 俯视角探索游戏
- 地图浏览系统
- 编辑器工具类项目

---

## 后续优化方向

- 支持触屏（移动端手势缩放 / 拖拽）
- 添加相机平滑过渡（Lerp / Damping）
- 限制相机移动边界（地图范围）
- 支持目标跟随（Follow Target）
- 多相机模式切换（自由视角 / 锁定视角）

---

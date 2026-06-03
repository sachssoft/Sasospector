# Sachssoft Sasospector

**Sachssoft Sasospector** is an inspector UI system. It works like a PropertyGrid, but is always extensible and controllable. It uses a schema system to define specific properties. It also supports reflection, so schemas can be generated automatically via reflection. The goal is to reduce the amount of boilerplate code required in many applications. A key focus is AOT support so that mobile platforms are supported as well. Reflection is only used when needed.

### ⚠️ Note
The inspector is primarily intended for MVVM systems. It acts as a layer between the Model and the ViewModel.

The schema is based on a model to register and describe its properties. This allows the UI (View) to read the properties in a structured way and automatically select appropriate editor components for rendering.

---

## ✨ Current Status

Sasospector is currently in development. The first alpha release is coming soon.

---

## 🧠 Purpose

Sasospector is designed to help developers:

- Reduce boilerplate code  
- Automatically detect property types in PropertyViewItems and assign the correct editor  
- Provide full control and customization  
- Minimize manual coding by leveraging reflection where appropriate  

---

## 📦 Basic

| Library | Status | Minimum .NET Version | NuGet |
|--------|--------|----------------------|------|
| Basic | Coming soon | 8.0 | |

---

## 🖥 UI

| Platform | Status | Minimum .NET Version | NuGet |
|----------|--------|----------------------|------|
| Avalonia | Coming soon | 8.0 | |
| WPF | Planned | 8.0 | |

---

## 🧩 Library

| Platform | Status | Minimum .NET Version | NuGet |
|----------|--------|----------------------|------|
| Skia | Coming soon | 8.0 | |
| Monogame | Coming soon | 8.0 | |
| FNA | Coming soon | 8.0 | |

---

## 🚀 Roadmap

Planned improvements include:

- Support for more target platforms (additional packages)  
- Long-term plan: WPF and others  

---

## 🤝 Contributing

Contributions, ideas, and feedback are welcome.
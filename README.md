# 🌲 FOREST HOLE ADVENTURE  
Set in a lush forest, **Forest Hole Adventure** is an exciting game where you play as a "hole," swallowing various objects and animals to grow bigger! 🎮✨  
🎯 **Goal:** Reach the target score before time runs out to complete the level!  

---
### 🎥 **Gameplay Video**  

https://github.com/user-attachments/assets/77ca87ad-58cb-4d85-b9cc-262e7417ad75

### 🖼️ **Screenshots**  

<p align="center">
  <img src="https://github.com/SERAP-KEREM/Hole/blob/main/Assets/GameImages/1.png?raw=true?raw=true" alt="Game Screenshot 1" width="200">
  <img src="https://github.com/SERAP-KEREM/Hole/blob/main/Assets/GameImages/2.png?raw=true?raw=true" alt="Game Screenshot 2" width="200">
   <img src="https://github.com/SERAP-KEREM/Hole/blob/main/Assets/GameImages/3.png?raw=true" alt="Game Screenshot 3" width="200">
</p>
<p align="center">
  <img src="https://github.com/SERAP-KEREM/Hole/blob/main/Assets/GameImages/4.png?raw=true" alt="Game Screenshot 4" width="200">
  <img src="https://github.com/SERAP-KEREM/Hole/blob/main/Assets/GameImages/6.png?raw=true" alt="Game Screenshot 5" width="200">
 <img src="https://github.com/SERAP-KEREM/Hole/blob/main/Assets/GameImages/5.png?raw=truee" alt="Game Screenshot 6" width="200">
</p>

---
## 📜 **Game Features**  

### 🔹 **Hole Mechanics**  
- 🌀 **Growth Mechanic**:  
  Objects are swallowed using shaders, and as the hole grows, it can consume larger objects!  
- 🎮 **Movement Mechanic**:  
  - 🕹️ Supports joystick and keyboard input.  
  - 🗺️ Smooth and dynamic movement powered by **NavMesh**.  
- 👁️‍🗨️ **Transparency Mechanic**:  
  Larger objects turn transparent when the hole passes beneath them.  

### 🔹 **Collectibles and Animal Mechanics**  
- ⭐ **Collectibles**:  
  Objects award points based on their size.  
- 🦌 **Animals**:  
  - Animals roam the map and try to escape when the hole gets close. 🏃‍♂️  
  - Animal movement is also powered by **NavMesh**.  

### 🔹 **Camera and Visuals**  
- 🎥 **Cinemachine Virtual Camera**:  
  - Tracks the hole and adjusts to show a larger area as the hole grows.  

### 🔹 **Level System**  
- 📄 **Scriptable Objects** allow each level to be customized:  
  - **Level Data**: Defines the target score, time limit, and object properties for each level.  
  - **Level Generator**: Determines which objects spawn on the map and how many.  

### 🔹 **Game Manager**  
- 🧠 Manages the entire game flow:  
  - Level transitions, 🎵 music settings, 🕹️ UI controls, and game states.  

---

## 📦 **Tools and Packages Used**  
- 🛠️ **TriInspector**: For a more organized and user-friendly Unity Inspector interface.  
- 🎞️ **DOTween**: For animations and movement mechanics.  
- 🎥 **Cinemachine**: For dynamic camera tracking and settings.  
- 🧰 **SerapkeremGameTools**:  
  - 🧩 MonoSingleton: For singleton class management.  
  - 🎮 InputManager: Joystick and keyboard-compatible input system.  
  - 💾 SaveLoadManager: For saving game progress.  
  - 🔊 AudioManager: For background music and sound effects.  
- 🗺️ **NavMesh**: Used for both hole and animal movements.  

---

## 🚀 **How to Play**  
1. 🕹️ Use **joystick or keyboard** to move the hole.  
2. ⭐ Swallow objects and animals to earn points.  
3. ⏱️ Reach the target score before time runs out to complete the level!  


---

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](https://github.com/SERAP-KEREM/SERAP-KEREM/blob/main/MIT%20License.txt) file for details.

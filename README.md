# 🌌 Universo Salvaje (Prototipo)

**Universo Salvaje** es un prototipo de *survival shooter espacial* desarrollado en Unity. El jugador controla una nave y debe sobrevivir en un entorno hostil, con enemigos que se generan de forma continua, incrementando su dificultad con el tiempo.

## 🎮 Descripción

En este juego tipo roguelike, no existen oleadas tradicionales. En su lugar, los enemigos aparecen dinámicamente, con diferentes tipos que se van desbloqueando a medida que avanza el tiempo. 

Los jugadores enfrentan:

- Naves enemigas con distintos comportamientos (explosivos, orbitantes, soporte, élites, etc.).
- Eventos especiales aleatorios que alteran el combate.
- Un **jefe final** desafiante que aparece al minuto 13.

### Enemigos principales:

- **Fighter**
- **Bomber** (tipo kamikaze)
- **Scout** (orbita y dispara)
- **Support Ship**
- **Torpedo Ship**
- **Frigate**
- **Battlecruizer Elite**
- **Boss Final**

## 🛠️ Tecnologías utilizadas

- [Unity](https://unity.com/) — Versión `6000.0.25f1`
- C#
- Pooling de enemigos
- ScriptableObjects para configuración modular

## 🎯 Objetivos de desarrollo en proceso

- [x] Sistema de generación continua con desbloqueo progresivo por tiempo
- [x] Pooling de enemigos con gestión de memoria
- [x] Sistema base de eventos especiales con cooldown
- [x] Lógica de aparición del jefe final al minuto 13
- [ ] Señalización visual y sonora para eventos especiales
- [ ] Ajuste de balance en la progresión de dificultad
- [ ] Mejora del UI: vida, tiempo restante, alertas
- [ ] Sistema de mejoras del jugador
- [ ] Música y efectos básicos de sonido

## 📁 Estructura del proyecto


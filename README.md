# Simulador de Nave Espacial con Arduino y Unity

## Autor(es)

**Hugo Alexander Fonseca Chaparro**  
Ingeniería en Multimedia

**Diana Sofía Benavides Monroy**  
Ingeniería en Multimedia

**Asignatura:** Realidad Mixta  
**Año:** 2025

---

## Descripción

Proyecto académico desarrollado en Unity y Arduino que combina simulación interactiva, exploración espacial y combate. El usuario puede controlar una nave espacial mediante controles físicos construidos con Arduino o mediante teclado, interactuando con diferentes sistemas de navegación y simulación dentro de la nave.

El proyecto busca aplicar conceptos de programación, simulación física, interacción humano-computador y comunicación entre hardware y software mediante experiencias inmersivas e interactivas.

---

## Características Principales

- Control mediante Arduino y teclado.
- Simulación de combate espacial.
- Exploración del interior de una nave.
- Sistema de gravedad dinámica.
- Sistema de escotilla con descompresión explosiva.
- Sistema de salud y destrucción de enemigos.
- Menús interactivos.
- Pantallas de pausa y Game Over.
- Sistema de objetivos.
- Indicadores de velocidad y estado de Boost.

---

## Escenarios Disponibles

### 1. Combate Espacial

El jugador controla una nave espacial y debe eliminar todos los enemigos presentes en el sector.

#### Características

- Disparo de proyectiles.
- Sistema de salud.
- Sistema de Boost.
- Contador de objetivos restantes.
- Pantalla de derrota.
- Cambio automático de escenario al completar el sector.

---

### 2. Exploración de la Nave

Permite manipular diferentes sistemas dentro de la nave.

#### Características

- Activación y desactivación de gravedad.
- Simulación de gravedad cero.
- Apertura y cierre de escotilla.
- Descompresión explosiva.
- Sistema de alerta de emergencia.

---

## Controles

### Control Arduino

#### Palanca Izquierda

Controla la elevación de la nave.

- Arriba → Ascender
- Abajo → Descender

#### Palanca Derecha

Controla la dirección de la nave.

- Abajo → Girar a la izquierda
- Arriba → Girar a la derecha

---

### Botón Izquierdo

Su función cambia dependiendo del contexto.

#### Menús

- Confirmar opción

#### Combate

- Disparar

#### Interior de la Nave

- Activar o desactivar gravedad
- Seleccionar opciones

---

### Botón Derecho

Su función cambia dependiendo del contexto.

#### Menús

- Cambiar selección

#### Combate

- Activar Boost

#### Interior de la Nave

- Abrir o cerrar escotilla

---

### Botón Central

#### Combate

- Pausar simulación

---

### Controles con Teclado

#### Combate Espacial

- W / S → Elevación
- A / D → Dirección
- Espacio → Disparar
- Shift → Boost
- Esc → Pausar

#### Exploración de la Nave

- G → Activar o desactivar gravedad
- Espacio → Abrir o cerrar escotilla

---

## Sistemas Implementados

### Sistema de Combate

- Generación de enemigos.
- Detección de impactos.
- Sistema de daño.
- Destrucción de objetivos.
- Gestión de sectores completados.

### Sistema de Salud

- Barra de vida.
- Daño progresivo.
- Pantalla de Game Over.

### Sistema de Boost

- Aumento temporal de velocidad.
- Sonido de activación.
- Indicador visual.

### Sistema de Gravedad

- Activación y desactivación dinámica.
- Simulación de gravedad cero.
- Modificación física de los objetos de la nave.

### Sistema de Escotilla

- Apertura y cierre animado.
- Descompresión explosiva.
- Atracción de objetos cercanos.
- Expulsión del usuario si se acerca demasiado.

---

## Tecnologías Utilizadas

- Unity 6
- C#
- Arduino
- TextMeshPro
- Sistema de Física de Unity

---

## Instrucciones de Ejecución

### Desde Unity

1. Abrir el proyecto mediante Unity Hub.
2. Abrir la escena principal.
3. Presionar Play.

### Desde Ejecutable

1. Abrir la carpeta Build.
2. Ejecutar el archivo `.exe`.
3. Conectar el Arduino antes de iniciar la simulación para utilizar los controles físicos.

---

## Objetivo Académico

Este proyecto fue desarrollado para la asignatura de Realidad Mixta con el objetivo de integrar conceptos de simulación interactiva, programación en Unity, diseño de interfaces, física aplicada y comunicación con dispositivos externos mediante Arduino.

---

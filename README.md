# Simulador de Nave Espacial con Arduino y Unity

**Estudiante:** Diana Sofía Benavides Monroy
**Asignatura:** Electiva y profundización I - Entornos y simulación multimedia

---

#### 1. Integración de 5 Simulaciones Físicas Coherentes (Sin Dependencia de RigidBody)

1. **Simulación de Órbitas Planetarias (Sistema Solar):** Movimiento de rotación alrededor de un punto central.
2. **Simulación de Rotación Intrínseca:** Movimiento rotacional independiente sobre los ejes propios de cada cuerpo celeste.
3. **Simulación de Atracción Gravitatoria por Esferas de Influencia (SOI):** Fuerza gravitacional que lleva a la nave hacia el centro del cuerpo celeste dependiendo de su rango y fuerza.
4. **Simulación de Entorno de Antigravedad:** Activación y desactivación de fuerza de gravedad dentro de un entorno cerrado, además de la posibilidad de cambiar la gravedad a la de diferentes planetas.
5. **Simulación de Descompresión Extrema (Escotilla):** Cambio drástico de presión en un entorno cerrado, al tener contacto con la presión del espacio, hace que todo lo que esté a su alrededor sea direccionado hacia su punto de succión.

---

#### 2. Sistema de Telemetría y modificación en tiempo real (HUD)

Se implementó un Panel de Control Dinámico interactivo y escalable que permite visualizar y alterar variables en tiempo real:
* **Datos calculados en pantalla:** Despliegue en vivo de la Masa del cuerpo celeste enfocado, Velocidad de órbita ($km/h$), Velocidad de rotación propia, Energía Cinética ($J$) y Energía Mecánica Total.
* **Control UI interactivo:** Sliders y Dropdowns funcionales para la manipulación en tiempo real del comportamiento de las simulaciones principales (Sistema Solar y Entorno de Antigravedad).

---

#### 3. Nuevos controles de entrada por teclado (Build estable)
Para garantizar la estabilidad absoluta durante la sustentación en vivo frente al jurado, se ha configurado un mapeo de teclado robusto para el control y navegación por el entorno 3D:
* **Tecla P:** Desplegar / Ocultar el panel de control dinámico de telemetría (UI visible e interactiva).
* **Tecla R:** Restablecer / Reiniciar la simulación física y limpiar las variables del entorno en cualquier momento.
* 
La navegación general en el entorno continúa siendo la presentada anteriormente.

---

# Simulador de Nave Espacial con Arduino y Unity

## Autor(es)

**Hugo Alexander Fonseca Chaparro**  
Ingeniería en Multimedia

**Diana Sofía Benavides Monroy**  
Ingeniería en Multimedia

**Asignatura:** Realidad Mixta  
**Año:** 2026

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

**Aclaración**
En el proyecto de Unity (Unity Editor), la funcionalidad entre el handcraft, el arduino y el código del proyecto funcionan de forma óptima. Debido a la sensibilidad del prototipo y a la cantidad de pruebas realizadas, el descargable funciona con algunas intermitencias en el botón de selección en la pantalla de pausa.
El commit que tiene el funcionamiento óptimo es el quinto, llamado "Se redujo el tiempo que debe pasar para que aparezca la alerta para diriguirse a la cabina del piloto, además se realizaron pruebas exitosas del funcionamiento del arduino con el handcraft terminado"

Adicionalmente, anexamos video en el que evidencia el buen funcionamiento del handcraft:
https://drive.google.com/file/d/1rHdW4-PMvmUUtfG8DXfvY9xIUZpQDWps/view?usp=sharing

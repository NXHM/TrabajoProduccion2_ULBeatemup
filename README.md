# Beatemup

¡Bienvenido a Beatemup! Un emocionante juego de lucha. Hecho por estudiantes de la Universidad de Lima. Ciclo 2024-2

# Requerimiento 1: Barra de Vida

1.**Descripción**: 
Este proyecto implementa un sistema de barras de vida tanto para el jugador como para los enemigos, utilizando técnicas y métodos inspirados en el video tutorial de Unity "Health Bars in Unity". El objetivo es gestionar de manera visual y funcional la vida de los personajes dentro del juego, permitiendo un mejor seguimiento de los puntos de vida restantes, tanto para el jugador como para los enemigos.
  - **Ejemplo Visual**: [Video de referencia](https://www.youtube.com/watch?v=BLfNP4Sc_iA)

2. **Implementación**
El sistema de barras de vida se desarrolló utilizando Canvas y componentes de Image en Unity, lo cual facilita la personalización visual y la gestión dinámica del estado de vida. Para cada personaje, se ha creado un script llamado Health.cs, encargado de almacenar, modificar y visualizar la vida.

 - Detalles Técnicos:
Barra de Vida del Jugador:
La barra de vida del jugador se actualiza cada vez que recibe daño por parte de los enemigos. En caso de que la vida llegue a cero, el jugador es automáticamente reaparecido en el punto inicial del nivel, reiniciando sus puntos de vida al valor máximo configurado.

 - Barras de Vida de los Enemigos:
Cada enemigo tiene una barra de vida independiente, que se actualiza en tiempo real al recibir daño. Además, se implementó un sistema de colores progresivos para las barras de los enemigos utilizando un Gradient, de modo que el color de la barra cambia visualmente según el nivel de vida restante.

 - Sistema de Daño:
Se ha implementado un sistema de colisión y detección de impactos, que permite a los enemigos y al jugador recibir daño y disminuir sus puntos de vida. El método TakeDamage(float amount) en el script Health.cs gestiona la reducción de vida y la actualización visual de las barras.

3. ### **Código**
El script Health.cs incluye métodos para recibir daño, actualizar la barra de vida y gestionar la muerte del personaje. Los puntos clave del código incluyen:
1. Método TakeDamage: Reduce los puntos de vida del personaje al recibir daño y actualiza visualmente la barra de vida.
2. Método UpdateHealthBar: Ajusta el relleno (fillAmount) y el color de la barra según el porcentaje de vida restante.
3. Método Die: Reaparece al jugador al inicio del nivel o destruye a los enemigos cuando su vida se agota.

4. ### **Comportamiento**:
 Jugador: La barra de vida del jugador disminuye gradualmente al recibir ataques. Si la vida llega a cero, el jugador reaparece en la posición inicial del nivel con su vida restaurada.
Enemigos: Cada enemigo posee su propia barra de vida, que cambia de color según la cantidad de vida restante. Cuando la vida de un enemigo se agota, este es destruido automáticamente.
Actualización Visual: La actualización visual de las barras es fluida, permitiendo a los jugadores seguir fácilmente el estado de vida de sus personajes y sus enemigos.

---

# Requerimiento 2: Ataques del Footsoldier

## Descripción

El footsoldier puede atacar de dos formas diferentes:

1. **Melee**: Ataque cuerpo a cuerpo, implementado durante las sesiones de clase.
2. **Shuriken**: Lanzamiento de proyectiles que causan daño variable al jugador.
3. **Spawner**: Para manejar que salgan enemigos cuando uno se de rotado

## Implementación de Ataques

### Melee

- **Descripción del Ataque**:  
  El ataque cuerpo a cuerpo se activa cuando el jugador se encuentra dentro del rango de `meleeDistance`, infligiendo daño directamente y manteniendo la presión durante el combate.

- **Implementación**:
  1. Se implementó el ataque melee y se animó el ataque del enemigo usando la animación **MeleeAttack.anim**.

     ![Animación de Ataque Melee](https://github.com/user-attachments/assets/1f13cd3a-9b34-4a36-85b2-fe5b1d719fa3)

  2. **Código**:
     - En el script `EnemyAttack.cs`, el ataque melee se implementa a través de los siguientes métodos:

       - **Método `PerformMeleeAttack()`**:
         - Se llama cuando el jugador está en rango.
         - Activa la animación mediante `TriggerMeleeAttack()` en `EnemyMovement`.
         - Llama a `ApplyMeleeDamage()` para infligir daño al jugador.
     
       - **Método `ApplyMeleeDamage()`**:
         - Verifica si el jugador está asignado.
         - Obtiene el componente `PlayerHealth` del jugador para aplicar el daño.
         - Registra el daño en la consola para seguimiento y depuración.

    - **Variables Relacionadas**:
       - `meleeDamage`: Cantidad de daño infligido por el ataque cuerpo a cuerpo.
       - `meleeDistance`: Rango dentro del cual se puede realizar el ataque.

  3. **Comportamiento**:  
     Al entrar en el rango de ataque, el enemigo debe:
       - Ejecutar la animación del ataque cuerpo a cuerpo.
       - Infligir daño al jugador.
       - Reflejar el cambio en la barra de vida del jugador si recibe daño.

---

### Implementación de Shuriken

- **Descripción del Ataque**:  
  El footsoldier puede lanzar shurikens a distancia, activándose cuando el jugador está a una distancia adecuada.

- **Características**:
  - El daño del ataque proyectil es variable.
  - Se implementará una lógica en el script para gestionar el lanzamiento y trayectoria del proyectil.

- **Implementación**:

  1. **Animación de Ataque a Distancia**:  
     ![Animación de Ataque a Distancia](https://github.com/user-attachments/assets/96f1e619-f9a1-42bd-8d01-960d0a176c0f)

  2. **Código**:
     
     2.1 **Clase `Shuriken`**:
     - Gestiona el comportamiento del proyectil lanzado. Se mueve hacia el objetivo y causa daño al jugador al colisionar. (Ubicación: `Assets/Scripts/Enemy/Shuriken/Shuriken.cs`)

     - **Variables**:
       - **`speed`**: Velocidad del proyectil (ajustable en el inspector de Unity).
       - **`maxLifetime`**: Tiempo máximo de vida del proyectil.
       - **`target`**: Referencia al jugador.
       - **`lifetime`**: Tiempo de vida restante del proyectil.
       - **`damage`**: Daño infligido al jugador.

     - **Métodos Clave**:
       - **`OnEnable()`**: 
         - Resetea el `lifetime` al `maxLifetime` cuando se activa.

       - **`Update()`**: 
         - Mueve el proyectil hacia el objetivo si está asignado.
         - Reduce el tiempo de vida. Si excede `maxLifetime`, se devuelve al pool.

       - **`SetTarget(Transform newTarget)`**: 
         - Asigna un nuevo objetivo al proyectil.

       - **`SetDamage(float distance, float maxDamage, float minDamage, float maxDistance)`**: 
         - Calcula el daño basado en la distancia usando interpolación lineal (`Mathf.Lerp`).

       - **`MoveTowardsTarget()`**: 
         - Calcula la dirección hacia el objetivo y mueve el proyectil usando `Rigidbody2D`.

       - **`OnTriggerEnter2D(Collider2D collision)`**: 
         - Detecta colisiones. Si colisiona con el jugador, aplica daño y devuelve el proyectil al pool.

       - **`ReturnToPool()`**: 
         - Desactiva el proyectil y lo devuelve al `ProjectilePoolManager`.

  2.2. **Clase `ProjectilePoolManager`**:
     - Gestiona un pool de proyectiles para optimizar la creación y reutilización de objetos. (Ubicación: `Assets/Scripts/Enemy/ProjectilePoolManager.cs`)

  2.3. **Cambios en la clase `EnemyAttack`**:

     - **`HandleAttack(float distance)`**:
       - **Descripción**: Gestiona la lógica de ataque del enemigo según la distancia al jugador.
       - **Condiciones de Ataque a Distancia**: Se ejecuta el ataque si la distancia es menor o igual a `shootDistance` y el tiempo actual supera el último ataque más el cooldown.

     - **`PerformRangeAttack(float distance)`**:
       - **Descripción**: Ejecuta un ataque a distancia.
       - **Funcionalidad**:
         - Imprime un mensaje en la consola sobre el ataque.
         - Llama a `FireProjectile(distance)` para lanzar un proyectil.
         - Actualiza `lastAttackTime` para iniciar el cooldown.

     - **`FireProjectile(float distance)`**:
       - **Descripción**: Gestiona la creación y lanzamiento de un proyectil.
       - **Funcionalidad**:
         - Verifica la disponibilidad de `projectilePoolManager` y el jugador.
         - Intenta obtener un proyectil del pool. Si se obtiene, se configura:
           - **Posición**: Se establece la posición inicial del proyectil.
           - **Objetivo**: Se asigna el jugador utilizando `SetTarget`.
           - **Daño**: Se calcula y asigna usando `SetDamage`.

  3. **Comportamiento**:
    - **Activación del Ataque**: 
       - El enemigo realiza un ataque a distancia cuando el jugador está dentro de `shootDistance` y el cooldown ha expirado.

    - **Disparo del Proyectil**:
       - Se obtiene un proyectil del pool, se posiciona y se asigna el jugador como objetivo.
       - Se calcula y activa el proyectil para que se mueva hacia el jugador.

    - **Gestión del Cooldown**:
       - Actualiza el tiempo del último ataque para evitar que el enemigo dispare nuevamente hasta que transcurra el cooldown.

---

### Documentación del EnemySpawner

#### Clase `EnemySpawner`

Clase que gestiona la aparición de enemigos en el juego. Permite definir el intervalo de generación, la posición y la cantidad máxima de enemigos activos.

1.- **Variables Privadas**

- **`enemiesSpawned`**: Contador de enemigos generados.
- **`enemiesDead`**: Contador de enemigos eliminados.

2.- **Propiedades Públicas**

- **`EnemiesSpawned`**: Número de enemigos generados.
- **`EnemiesDead`**: Número de enemigos eliminados.

3.- **Métodos Principales**

- **`Start()`**: Inicializa la generación de enemigos.
- **`SpawnEnemies()`**: Coroutine que genera enemigos en intervalos específicos.
- **`GetValidSpawnPosition()`**: Busca una posición válida para la generación de un enemigo.
- **`GetRandomPointInCameraView()`**: Genera un punto aleatorio en la vista de la cámara.
- **`IsPositionValid(Vector2 position)`**: Verifica si una posición es válida para generar un enemigo.
- **`EnemyDied()`**: Registra la muerte de un enemigo y actualiza el contador correspondiente.



---

## Requerimiento 3: Implementación del Boss

- **Descripción**: Introducir un boss con dos formas de ataque, cada una con daño variable al jugador.

- **Características**:
  - **Cinemática de Introducción**: Antes de que aparezca el boss, debe haber una secuencia cinemática que introduzca al personaje.
  - **Fijación de Cámara**: Durante la introducción y el combate, la cámara debe permanecer estática.
  - **Barra de Vida del Boss**: Debe situarse en la parte inferior de la pantalla, actualizándose al recibir daño.
  
- **Comportamiento**:
  - Al morir el boss, debe haber una secuencia cinemática final que cierre el juego.

### Cinemática de Introducción y Salida
1. **Descripción**:  
Este proyecto implementa una mecánica de cinemática que introduce y concluye la batalla contra el jefe (boss). La cinemática inicial se activa al entrar en una zona específica, mientras que la cinemática final se ejecuta automáticamente al derrotar al boss. Estas cinemáticas están diseñadas para centrarse en el jefe como el foco principal, desactivando el control del jugador temporalmente durante la secuencia.

2. **Implementación**:  
Las cinemáticas se logran utilizando GameObjects que contienen los componentes `CinemachineVirtualCamera` y `PlayableDirector`, los cuales permiten reproducir secuencias enfocadas en el boss. La cinemática inicial se activa cuando el jugador entra en una zona determinada, mientras que la cinemática final se activa al morir el boss.

   - **Detalles Técnicos**:
     - **Cinemática Inicial**:
       - **Trigger de Activación**: Se utiliza un `BoxCollider 2D` como trigger. Al detectar la colisión del jugador con el trigger, se activa la cinemática de introducción.
       - **Desactivación del Jugador**: Durante la cinemática, el control del jugador se desactiva temporalmente para centrar la atención en el boss.
  
     - **Cinemática Final**:
       - **Activación Post-Boss**: La cinemática final se ejecuta una vez que el boss es derrotado, poniendo fin al combate y mostrando el desenlace de la batalla.

     - **Componentes Principales**:
       - **CinemachineVirtualCamera**: Cámara virtual que enfoca al boss durante las cinemáticas.
       - **PlayableDirector**: Controla la reproducción de la secuencia de la cinemática.
       - **Trigger Collider**: Un `BoxCollider 2D` en modo trigger inicia la cinemática de introducción cuando el jugador lo cruza.

3. ### **Código**:
El script encargado de activar la cinemática inicial utiliza un `BoxCollider 2D` en modo trigger. Cuando el jugador entra en esta zona, el script detecta la colisión y ejecuta las siguientes acciones:
   1. **OnTriggerEnter2D(Collider2D other)**: Este método verifica si el objeto que entró en el área del trigger es el jugador (mediante la etiqueta "Player"). Si es así, se activa el jefe (`boss.SetActive(true)`), se reproduce la cinemática utilizando el componente `PlayableDirector` (`m_PlayableDirector.Play()`), y se desactiva el GameObject del trigger (`gameObject.SetActive(false)`) para evitar que la cinemática se repita.
Este flujo permite iniciar la cinemática automáticamente al entrar en el área del jefe, asegurando que la introducción solo ocurra una vez.


4. ### **Comportamiento**:
   - **Cinemática Inicial**: Se reproduce cuando el jugador entra en la zona de activación (trigger), enfocando al boss con la cámara virtual y desactivando temporalmente al jugador.
   - **Cinemática Final**: Se ejecuta tras derrotar al boss, mostrando el desenlace de la batalla y concluyendo la secuencia del jefe.

Este sistema de cinemáticas añade una capa cinematográfica que mejora la inmersión del jugador, proporcionando un enfoque visual dramático tanto en la introducción como en la salida del combate contra el boss.

### Barra de Vida del Boss
1. **Descripción**:  
Este proyecto incorpora una barra de vida dinámica para el jefe (boss) del juego, diseñada para reflejar el estado de salud en todo momento durante el combate. Al morir el jefe, la barra de vida desaparece, indicando el final del enfrentamiento. Esta funcionalidad está inspirada en sistemas de vida de juegos avanzados y busca ofrecer una experiencia visual clara para los jugadores.
  
2. **Implementación**:  
La barra de vida del boss se desarrolla mediante un Canvas, en el cual se incluye el componente de la barra de vida. Dentro de este Canvas, se emplean dos imágenes: una como el marco de la barra y otra para el relleno que muestra el nivel de vida restante, utilizando un gradiente que cambia según la vida del jefe. La barra desaparece automáticamente cuando el jefe es derrotado.

   - **Detalles Técnicos**:
     - **Barra de Vida del Boss**:
       - **Canvas y Componentes**: La barra de vida del boss está compuesta por un Canvas que contiene dos imágenes: un marco decorativo y un relleno que cambia progresivamente de color de acuerdo al porcentaje de vida restante.
  
     - **Métodos Principales**:
       - **SetHealthMax**: Establece el valor máximo de vida del boss al comienzo del combate.
       - **SetHealth**: Ajusta visualmente la barra de vida en tiempo real según la salud restante del boss.
       - **TakeDamage**: Reduce la vida del boss cuando recibe daño y actualiza la barra visualmente. Cuando la vida llega a cero, la barra de vida desaparece.

3. ### **Código**:
El script `BossHealth.cs` incluye los siguientes métodos:
   1. **SetHealthMax()**: Configura el valor máximo de vida y asegura que la barra de vida esté llena al comienzo.
   2. **SetHealth()**: Actualiza la barra según la salud restante, ajustando el `fillAmount` y aplicando el gradiente.
   3. **TakeDamage(float amount)**: Reduce la salud del boss y actualiza la barra. Si la salud llega a cero, se invoca la desaparición de la barra de vida.

4. ### **Comportamiento**:
   - **Boss**: La barra de vida del boss siempre está visible y se reduce a medida que recibe daño. Cuando la vida llega a cero, la barra desaparece del HUD, y el boss es derrotado.
   - **Actualización Visual**: La barra muestra un cambio gradual de color a medida que la vida del boss disminuye, brindando al jugador una referencia clara del estado del combate.

Este sistema asegura una experiencia de combate fluida, donde los jugadores pueden monitorear fácilmente la vida del boss y el progreso de la batalla.

### Comportamiento del Boss
El script `Boss.cs` incluye el comportamiento del boss

1.- **Variables Privadas**

- **`tiempoEspera`**: Frecuencia mínima y máxima en la que el Boss se queda quieto.
- **`velSalto`**: Qué tan rápido el Boss salta y cae.
- **`tiempoAire`**: Cuánto tiempo el Boss pasa en el aire antes de caer al piso
- **`piso`**: Apunta al Transform del piso del Boss
- **`bossSprite`**: Apunta al Transform del sprite del Boss
- **`anim`**: Apunta al Animator del sprite del Boss
- **`col`**: Apunta al hitbox del Boss
- **`colDamage`**: Apunta al collider donde el Boss golpea al Jugador
- **`esperaSalto`**: Booleano para que el script sepa si el Boss ya puede saltar
- **`m_PlayableDirector`**: Apunta al PlayableDirector del Boss

2.- **Propiedades Públicas**

- **`maxHealth`**: Vida máxima.
- **`currentHealth`**: Vida actual.
- **`healthBar`**: Apunta al script Healthbar del Boss

3.- **Métodos Principales**

- **`OnValidate()`**: Previene el ingreso de parámetros incorrectos desde el editor
- **`Start()`**: Asigna la vida máxima y llama a la corrutina `Behaviour`
- **`TakeDamage()`**: Hace que el Boss sufra daño, y si se le acaba, muere
- **`EsperaSalto()`**: Llamada desde el Animator, para avisar que el Boss ya está parado y puede comenzar a saltar

4.- **Corrutinas**

- **`Behaviour()`**: Se encarga de esperar y tras cierto tiempo, llamar a los ataques. El Boss tiene un animación en bucle Idle
- **`Attack1()`**: El Boss se para y comienza a saltar sobre el escenario, pudiendo golpear al jugador con su aterrizaje. Tras algunos saltos, vuelve a quedarse inmóvil y regresa a `Behaviour()`
- **`Jump()`**: Se encarga de que el Boss salte
- **`Fall()`**: Se encarga de que el Boss caiga al piso
- **`Land()`**: Se encarga de que el Boss caiga al piso una última vez

### EnemyHitbox del Boss
Detecta si el Boss fue golpeado por el Player, y llama al script `Boss.cs` para que sufra daño
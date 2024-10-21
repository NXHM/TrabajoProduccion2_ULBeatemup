# Beatemup

¡Bienvenido a Beatemup! Un emocionante juego de lucha. Hecho por estudiantes de la Universidad de Lima. Ciclo 2024-2

# Requerimiento 1: Barra de Vida

- **Descripción**: Implementar una barra de vida para el jugador que descienda cada vez que reciba ataques de los enemigos.
  - **Ejemplo Visual**: [Video de referencia](https://www.youtube.com/watch?v=BLfNP4Sc_iA)
  
- **Características**:
  - La barra de vida debe ser visible en pantalla y actualizarse en tiempo real.
  - Cada enemigo también debe tener su propia barra de vida, visible en su posición.
  - El color de la barra de vida de los enemigos debe cambiar dependiendo del nivel de daño recibido (por ejemplo: verde para daño bajo, amarillo para daño medio, rojo para daño alto).

- **Comportamiento**:
  - Cuando el jugador muera, debe reaparecer al inicio del nivel.

---

# Requerimiento 2: Ataques del Footsoldier

## Descripción

El footsoldier puede atacar de dos formas diferentes:

1. **Melee**: Ataque cuerpo a cuerpo, implementado durante las sesiones de clase.
2. **Shuriken**: Lanzamiento de proyectiles que causan daño variable al jugador.

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

## Requerimiento 3: Implementación del Boss

- **Descripción**: Introducir un boss con dos formas de ataque, cada una con daño variable al jugador.

- **Características**:
  - **Cinemática de Introducción**: Antes de que aparezca el boss, debe haber una secuencia cinemática que introduzca al personaje.
  - **Fijación de Cámara**: Durante la introducción y el combate, la cámara debe permanecer estática.
  - **Barra de Vida del Boss**: Debe situarse en la parte inferior de la pantalla, actualizándose al recibir daño.
  
- **Comportamiento**:
  - Al morir el boss, debe haber una secuencia cinemática final que cierre el juego.

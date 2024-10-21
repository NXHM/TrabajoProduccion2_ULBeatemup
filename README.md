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

Permitir que el footsoldier ataque de dos formas diferentes:
1. **Melee**: Ataque cuerpo a cuerpo, implementado durante las sesiones de clase.
2. **Shuriken**: Lanzamiento de proyectiles que también causan daño variable al jugador.

## Implementación de Ataques

### Melee

- **Descripción del Ataque**:  
  El ataque cuerpo a cuerpo se activa cuando el jugador se encuentra dentro del rango de `meleeDistance`. Este ataque inflige daño directamente al jugador y es esencial para mantener la presión durante el combate.

- **Implementación**:
  1. Se realizó la implementación del ataque melee, para lo cual se animó el ataque del enemigo. La animación correspondiente es **MeleeAttack.anim**.

     ![Animación de Ataque Melee](https://github.com/user-attachments/assets/1f13cd3a-9b34-4a36-85b2-fe5b1d719fa3)
  
  2. **Código**:
     - En el script `EnemyAttack.cs`, se implementa el ataque melee a través del siguiente método:

       - **Método `PerformMeleeAttack()`**:
         - Este método se llama cuando el jugador está en rango para un ataque cuerpo a cuerpo.
         - Activa una animación del enemigo a través del método `TriggerMeleeAttack()` del componente `EnemyMovement`.
         - Luego, llama a `ApplyMeleeDamage()` para infligir daño al jugador.
     
       - **Método `ApplyMeleeDamage()`**:
         - Verifica si el jugador está asignado.
         - Obtiene el componente `PlayerHealth` del jugador para aplicar el daño.
         - Registra el daño infligido en la consola para seguimiento y depuración.

    - **Variables Relacionadas**:
       - `meleeDamage`: Determina la cantidad de daño infligido por el ataque cuerpo a cuerpo.
       - `meleeDistance`: Establece el rango dentro del cual el ataque cuerpo a cuerpo puede ser realizado.

  3. **Comportamiento**:  
     Cuando el jugador entra en el rango de ataque, el enemigo debe:
       - Ejecutar la animación del ataque cuerpo a cuerpo.
       - Infligir el daño correspondiente al jugador.
       - Reflejar este cambio en la barra de vida del jugador si recibe daño.

---

### Implementación de Shuriken

- **Descripción del Ataque**:  
  El footsoldier puede lanzar shurikens como ataque a distancia. Este ataque se activa cuando el jugador está a una distancia adecuada.

- **Características**:
  - El daño infligido por el ataque de proyectil también debe ser variable.
  - Se implementará una lógica en el script para gestionar el lanzamiento y la trayectoria del proyectil.

- **Implementación**:

  1. **Código**:
     - La clase **`Shuriken`** gestiona el comportamiento del proyectil lanzado por el footsoldier. El shuriken se mueve hacia el objetivo y causa daño al jugador al colisionar con él. (Ubicación: `Assets/Scripts/Enemy/Shuriken/Shuriken.cs`)

     - **Variables**:
       - **`speed`**: Velocidad del proyectil. Se ajusta en el inspector de Unity.
       - **`maxLifetime`**: Tiempo máximo de vida del proyectil antes de ser reciclado.
       - **`target`**: Referencia al objeto objetivo (normalmente el jugador).
       - **`lifetime`**: Tiempo de vida restante del proyectil.
       - **`damage`**: Daño que el proyectil infligirá al jugador.

     - **Métodos Clave**:
       - **`OnEnable()`**: 
         - Resetea el tiempo de vida del proyectil (`lifetime`) al valor de `maxLifetime` cuando se activa.

       - **`Update()`**: 
         - Mueve el proyectil hacia el objetivo si hay uno asignado.
         - Reduce el tiempo de vida del proyectil. Si excede `maxLifetime`, el proyectil se devuelve al pool.

       - **`SetTarget(Transform newTarget)`**: 
         - Asigna un nuevo objetivo al proyectil, permitiendo que se dirija al jugador.

       - **`SetDamage(float distance, float maxDamage, float minDamage, float maxDistance)`**: 
         - Calcula el daño basado en la distancia entre el enemigo y el jugador utilizando interpolación lineal (`Mathf.Lerp`).

       - **`MoveTowardsTarget()`**: 
         - Calcula la dirección hacia el objetivo y mueve el proyectil usando el componente `Rigidbody2D`.

       - **`OnTriggerEnter2D(Collider2D collision)`**: 
         - Detecta la colisión del proyectil con otros objetos.
         - Si colisiona con el jugador, aplica daño usando el componente `PlayerHealth` y devuelve el proyectil al pool.

       - **`ReturnToPool()`**: 
         - Desactiva el proyectil y lo devuelve al `ProjectilePoolManager`.

  2. **Clase `ProjectilePoolManager`**:
     - Esta clase gestiona un pool de proyectiles para optimizar la creación y reutilización de objetos, evitando la sobrecarga de memoria y mejorando el rendimiento. (Ubicación: `Assets/Scripts/Enemy/ProjectilePoolManager.cs`)

  3. **En la clase `EnemyAttack` se muestran los siguientes cambios relacionados con el ataque a distancia**:

     - **`HandleAttack(float distance)`**:
       - **Descripción**: Gestiona la lógica de ataque del enemigo según la distancia al jugador.
       - **Condiciones de Ataque a Distancia**: Si la distancia al jugador es menor o igual a `shootDistance` y el tiempo actual supera el tiempo del último ataque más el cooldown, se ejecuta el ataque a distancia.

     - **`PerformRangeAttack(float distance)`**:
       - **Descripción**: Este método se encarga de ejecutar un ataque a distancia.
       - **Funcionalidad**:
         - Imprime un mensaje en la consola indicando que se está disparando un proyectil.
         - Llama al método `FireProjectile(distance)` para lanzar un proyectil hacia el jugador.
         - Actualiza `lastAttackTime` al tiempo actual para iniciar el cooldown del próximo ataque.

     - **`FireProjectile(float distance)`**:
       - **Descripción**: Gestiona la creación y el lanzamiento de un proyectil.
       - **Funcionalidad**:
         - Comprueba si el `projectilePoolManager` y el jugador están disponibles.
         - Intenta obtener un proyectil del pool. Si se obtiene, se configura:
           - **Posición**: Se establece la posición inicial del proyectil en relación con el enemigo.
           - **Objetivo**: Se asigna el jugador como objetivo utilizando el método `SetTarget` del script `Shuriken`.
           - **Daño**: Se calcula y asigna el daño utilizando `SetDamage`, tomando en cuenta la distancia actual entre el enemigo y el jugador.

  4. **Comportamiento**:
    - **Activación del Ataque**: 
       - Cuando el jugador se encuentra dentro de `shootDistance` y el cooldown ha expirado, el enemigo realiza un ataque a distancia.

    - **Disparo del Proyectil**:
       - Se obtiene un proyectil del pool y se posiciona adecuadamente.
       - Se asigna el jugador como objetivo del proyectil.
       - Se calcula el daño del proyectil en función de la distancia al jugador.
       - Se activa el proyectil, que comienza a moverse hacia el jugador.

    - **Gestión del Cooldown**:
       - Una vez que se ha realizado el ataque, el tiempo del último ataque se actualiza, evitando que el enemigo dispare nuevamente hasta que transcurra el cooldown.

---

## Requerimiento 3: Implementación del Boss

- **Descripción**: Introducir un boss con dos formas de ataque, cada una con daño variable al jugador.

- **Características**:
  - **Cinemática de Introducción**: Antes de que aparezca el boss, debe haber una secuencia cinemática que introduzca al personaje.
  - **Fijación de Cámara**: Durante la introducción y el combate, la cámara debe permanecer estática.
  - **Barra de Vida del Boss**: Debe situarse en la parte inferior de la pantalla, actualizándose al recibir daño.
  
- **Comportamiento**:
  - Al morir el boss, debe haber una secuencia cinemática final que cierre el juego.

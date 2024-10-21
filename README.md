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
       - En caso de que el jugador reciba daño, el sistema debe reflejar este cambio en su barra de vida.

### Implementación de Shuriken

- **Descripción del Ataque**:  
  El footsoldier puede lanzar shurikens o kunais como ataque a distancia. Este ataque se activa cuando el jugador está a una distancia adecuada.

- **Características**:
  - El daño infligido por el ataque de proyectil también debe ser variable.
  - Implementar una lógica en el script para gestionar el lanzamiento y la trayectoria del proyectil.

- **Implementación**:
  
  2. **Código**:
  - La clase **`Shuriken`** gestiona el comportamiento del proyectil lanzado por el footsoldier. El shuriken se mueve hacia el objetivo y causa daño al jugador al colisionar con él. (Assets/Scripts/Enemy/Shuriken/Shuriken.cs)
  - Variables:
    - **`speed`**: Velocidad del proyectil. Se ajusta en el inspector de Unity.
    - **`maxLifetime`**: Tiempo máximo de vida del proyectil antes de ser reciclado.
    - **`target`**: Referencia al objeto objetivo (normalmente el jugador).
    - **`lifetime`**: Tiempo de vida restante del proyectil.
    - **`damage`**: Daño que el proyectil infligirá al jugador.

   - **`OnEnable()`**
      - Resetea el tiempo de vida del proyectil (`lifetime`) al valor de `maxLifetime` cuando se activa.

   - **`Update()`**
      - Mueve el proyectil hacia el objetivo si hay uno asignado.
      - Reduce el tiempo de vida del proyectil. Si excede `maxLifetime`, el proyectil se devuelve al pool.

   - **`SetTarget(Transform newTarget)`**
      - Asigna un nuevo objetivo al proyectil, permitiendo que se dirija al jugador.

   - **`SetDamage(float distance, float maxDamage, float minDamage, float maxDistance)`**
      - Calcula el daño basado en la distancia entre el enemigo y el jugador utilizando interpolación lineal (`Mathf.Lerp`).

   -  **`MoveTowardsTarget()`**
      - Calcula la dirección hacia el objetivo y mueve el proyectil usando el componente `Rigidbody2D`.

   - **`OnTriggerEnter2D(Collider2D collision)`**
      - Detecta la colisión del proyectil con otros objetos.
      - Si colisiona con el jugador:
        - Aplica daño usando el componente `PlayerHealth`.
        - Devuelve el proyectil al pool.

   -  **`ReturnToPool()`**
      - Desactiva el proyectil y lo devuelve al `ProjectilePoolManager`.
    
   La clase `ProjectilePoolManager` gestiona un pool de proyectiles para optimizar la creación y reutilización de objetos, evitando la sobrecarga de memoria y mejorando el rendimiento. (Assets/Scripts/Enemy/ProjectilePoolManager.cs)

---

## Requerimiento 3: Implementación del Boss

- **Descripción**: Introducir un boss con dos formas de ataque, cada una con daño variable al jugador.

- **Características**:
  - **Cinemática de Introducción**: Antes de que aparezca el boss, debe haber una secuencia cinemática que introduzca al personaje.
  - **Fijación de Cámara**: Durante la introducción y el combate, la cámara debe permanecer estática.
  - **Barra de Vida del Boss**: Debe situarse en la parte inferior de la pantalla, actualizándose al recibir daño.
  
- **Comportamiento**:
  - Al morir el boss, debe haber una secuencia cinemática final que cierre el juego.

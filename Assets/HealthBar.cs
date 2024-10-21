using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con Sliders y componentes UI

namespace GameUI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider; // Componente UI del Slider
        public Gradient gradient; // Para cambiar los colores
        public Image fill; // Imagen de relleno para cambiar el color

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
            fill.color = gradient.Evaluate(1f); // Máxima vida
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue); // Actualizar color en base al porcentaje
        }
    }
}

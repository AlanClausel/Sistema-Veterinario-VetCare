from PIL import Image
import numpy as np

# Cargar la imagen original nuevamente (desde backup o recrear)
# Como ya modificamos la original, vamos a trabajar con lo que tenemos
# y ajustar los colores con mejor contraste

# Reabrir la imagen original desde un path temporal
# Primero intentemos recrear desde cero o usar una copia

print("Descargando imagen original nuevamente...")

# Vamos a crear el logo con colores verdosos desde la imagen que ya modificamos
# Pero esta vez con mejor contraste

img = Image.open('LogoVetCare.png').convert('RGBA')
data = np.array(img)

# Colores del tema verdoso del formulario
FONDO = (224, 242, 241)  # Verde agua muy claro (fondo del form)
VERDE_OSCURO = (38, 166, 154)  # Verde turquesa (texto y contornos)
VERDE_MEDIO = (120, 200, 190)  # Verde medio (elementos decorativos)
VERDE_CLARO = (180, 230, 220)  # Verde claro (rellenos)

# Crear nueva imagen con colores ajustados
new_data = data.copy()

for y in range(data.shape[0]):
    for x in range(data.shape[1]):
        r, g, b, a = data[y, x]

        # Ignorar píxeles muy transparentes
        if a < 10:
            continue

        # Calcular brillo del píxel original
        brightness = (int(r) + int(g) + int(b)) / 3

        # Mapear según el brillo original para mantener los detalles
        if brightness < 80:  # Muy oscuro (contornos, texto)
            new_data[y, x] = (*VERDE_OSCURO, a)
        elif brightness < 140:  # Oscuro medio (detalles)
            new_data[y, x] = (60, 180, 165, a)
        elif brightness < 190:  # Medio (elementos decorativos)
            new_data[y, x] = (*VERDE_MEDIO, a)
        elif brightness < 230:  # Claro (rellenos)
            new_data[y, x] = (*VERDE_CLARO, a)
        else:  # Muy claro (fondo)
            new_data[y, x] = (*FONDO, a)

# Guardar la nueva imagen
new_img = Image.fromarray(new_data, 'RGBA')
new_img.save('LogoVetCare.png')

print("Logo modificado exitosamente con colores verdosos y buen contraste")
print(f"  - Fondo: RGB{FONDO}")
print(f"  - Contornos/Texto: RGB{VERDE_OSCURO}")
print(f"  - Detalles: RGB{VERDE_MEDIO}")

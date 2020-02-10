Resumen
=======

En esta prueba de programación se ha desarrollado un clon del juego Arkanoid en Unity 3D 2019.2.0a11 incluyendo las siguientes funcionalidades.

-   Una pala que se mueve de izquierda a derecha dentro de los límites de la pantalla.

-   Una bola lanzable desde la pala que rebota en los bordes derecho, izquierdo y superior, en la pala y en los ladrillos.

-   La bola desaparece y se resetea en su posición inicial (el centro de la pala) al impactar en el borde inferior.

-   Ladrillos de cuatro tipo: indestructibles y destruibles por uno, dos y tres golpes de la bola.

-   Generador de niveles en base a entrada de texto.

-   Sistema de puntuación.

-   Sistema de vidas.

-   Cápsulas modificadoras de cuatro tipos: aumento y disminución de velocidad de bola, aumento y disminición del tamaño de la pala.

El tiempo total dedicado ha sido aproximadamente de 8 horas. El detalle de el tiempo de desarrollo para cada parte puede verse en el siguiente desglose:

-   **[ball](Bola):** 0.5 horas.

-   **[racket](Pala):** 0.5 horas.

-   **[bricks](Ladrillos):** 1 hora.

-   **[capsules](Cápsulas modificadoras) :** 1 hora.

-   **[levels](Generador de niveles) :** 2 horas.

-   **[gamenager](GameManager):**

    -   **Flujo de juego:** 2 hora.

    -   **Puntuaciones:** 0.5 horas.

    -   **GUI:** 0.5 horas.

-   **Memoria:** 1 horas.

Puede encontrarse el código descargable en el siguiente repositorio: <https://github.com/HernanMGC/ArkanoidXaloc>.

<a name="ball"></a>Bola
====

Para el desarrollo de la bola se ha creado un script llamado `BallMovement.cs` que controla su movimiento y su impacto con los impactos con aquellos GameObjects que tengan el script que herede de `Hitable.cs`. Dicho script permite definir las reacciones de un GameObject al recibir un impacto, consta de un atributo `public` de tipo `enum` para definir el tipo de reacción y una función `public virtual` `Hit`.

El script `Hitable` se ha heredado y aplicado a los bordes del juego, a los ladrillos y a la pala.

La bola mientras pueda moverse y su impacto con objetos no la destruya rebotará haciendo un reflejo dada la normal con el `Collider` que impacte. Un caso especial es el del rebote con ladrillos, dado que esta implementación podía provocar que impactara en un mismo frame con dos `Colliders` y generar dos reflejos que se invalidarían entre sí.

Adicionalmente, para aumentar el control de la bola por parte del jugador el impacto de la bola con la pala añade fuerza en la componente X del movimiento en función del punto de impacto con la la pala como puede verse en la imagen siguiente.

![Impacto con la pala](/readmeImages/racketImpact.jpg)

<a name="racket"></a>Pala
====

Para el desarrollo de la pala se han creado dos scripts: uno llamado `CharacterMovement.cs`, encargado del movimiento de la pala y el lanzamiento de la bola, y otro llamado `Racket.cs` que hereda de `Hitable.cs`, encargado la gestión de colisiones y del efecto de las capsulas modificadoras.

El script encargado del movimiento de la pala establece mediante la función `InitializaBoundaries` los límites de movimiento horizontal en función del tamaño del `Collider`. Esta función se llama cada vez que el efecto de alguna cápsula hace variar el tamaño de la pala.

El script encargado de la pala como elemento de juego `Hitable` también se encarga del reseteo de posición de la pala y de la modificación en tamaño, tanto de `Sprite` como de `Collider`, por efecto de las cápsulas.

<a name="bricks"></a>Ladrillos
=========

Para el desarrollo de los ladrillos se ha creado un script llamado `Brick.js` que hereda de `Hitable.js`. En este caso, al ser destruido el objeto por recibir un golpe genera una llamada la función `BrickDestroyed` de `GameManager.js` lanzando los eventos de cambio en el juego: subida de puntuación, reducción del contador de ladrillos restantes para pasar el nivel y la posible generación de una cápsula modificadora.

<a name="capsules"></a>Cápsulas modificadoras
======================

Para el desarrollo de las cápsulas se ha creado un script llamado `Capsule.js` que define un función `public virtual` `ApplyEffect` que ha de ser implementada por los scripts que hereden de él. En esta entrega se han desarrollado cuatro tipos de cápsulas con cuatro efectos distintos y para cada ella se ha generado un `Prefab` con su sprite y aceleración de caída propia.

Dentro del `GameManager` hay una sección de configuración de las cápsulas, donde puede definirse qué capsulas pueden a aparecer o no, cuál es la probabilidad de que aparezca una cápsula al romper un ladrillo y, en caso de aparecer alguna cápsula, qué peso tiene cada cápsula para aparecer.

<a name="levels"></a>Generador de niveles
====================

Para el desarrollo de la generación y definición de niveles se ha añadido a `GameManager.js` una sección de configuración en la que mediante una lista de `TextAreas` pueden definirse cada uno de los niveles del juego. La forma de generar un nivel se base en al parseo del texto: cada línea del `TextArea` representa una línea de ladrillos, sólo se toman en cuenta los diez primeros caracteres de cada línea y en función del caracter usado se instancia un ladrillo de uno, dos o tres golpes, un ladrillo irrompible o nada.

La relación de caracteres y ladrillos es la siguiente:

-   **U:** Ladrillo irrompible.

-   **1:** Ladrillo de un golpe.

-   **2:** Ladrillo de dos golpes.

-   **3:** Ladrillo de tres golpes.

-   **Cualquier otro caracter:** Sin ladrillo.

<a name="gamenager"></a>GameManager
===========

Todo la gestión de inicializaciones de niveles, posicionamiento de elementos, actualización de GUI está orquestada por el script `GameManager.js` sirviéndose de las funciones públicas de cada uno de los elementos de juego.

Se ha modificado el editor de este componente para poder pemitir una gestión más sencilla a los diseñadoras. Se ha creado un script `EditorList.js` para mejorar la visualización de atributos públicos en forma de listas que puedan ser representados con una estructura de datos, ejemplo de esto son la lista de `CapsulePrize`, la lista de sprites para los ladrillos y la lista de `ArkanoidLevel`.

Para cada elemento de una lista se permite duplicar, eliminar y desplazar hacia abajo.

![Lista de `CapsulePrize`>](/readmeImages/editorList.jpg)

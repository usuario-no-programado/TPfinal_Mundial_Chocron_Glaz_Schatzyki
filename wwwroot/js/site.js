function mostrarContenido() {
  const contenido = document.getElementById("contenido");
  const sobre = document.getElementById("sobre");
  const imagenPaquete = sobre.querySelector("img");
  const boton = sobre.querySelector("button");
  
  // Desactivar el botón para evitar clics múltiples
  boton.disabled = true;
  
  // Agregar animación de aplastamiento a la imagen
  imagenPaquete.classList.add("aplastando");
  
  // Esperar a que termine la animación del paquete (0.8s)
  setTimeout(() => {
    // Ocultar el sobre y mostrar contenido
    contenido.classList.remove("oculto");
    sobre.classList.add("oculto");
    
    // Obtener todas las figuritas (solo las que se ven, no los inputs ocultos)
    const figuritas = contenido.querySelectorAll(".figuritas.paquete:not(:has(input))");
    
    // Animar cada figurita con un delay
    figuritas.forEach((figurita, index) => {
      // Generar un ángulo random entre -8 y 8 grados
      const anguloRandom = (Math.random() * 4) - 2;
      figurita.style.setProperty('--rotation', `${anguloRandom}deg`);
      
      // Reset animation
      figurita.classList.remove("animar");
      // Trigger reflow to restart animation
      void figurita.offsetWidth;
      // Agregar clase de animación con delay
      figurita.style.animationDelay = (index * 0.1) + "s";
      figurita.classList.add("animar");
    });
  }, 800);
}

function getNumeroPagina() {
  const numeroPagina = document.getElementById("numeroPagina");
  return parseInt(numeroPagina.textContent.split(" ")[1], 10);
}

function updateNumeroPagina(pagina) {
  const numeroPagina = document.getElementById("numeroPagina");
  numeroPagina.textContent = `Pagina ${pagina}`;
}

function showPagina(pagina) {
  const paginas = document.querySelectorAll('section[id^="pag"]');
  const totalPaginas = paginas.length;
  const paginaActual = Math.min(Math.max(pagina, 1), totalPaginas);
  const current = document.querySelector('section[id^="pag"].visible');
  const target = document.getElementById(`pag${paginaActual}`);

  if (!target) {
    return;
  }

  if (current && current.id === target.id) {
    updateNumeroPagina(paginaActual);
    return;
  }

  function showTarget() {
    if (current) {
      current.classList.remove('visible', 'fade-out');
      current.classList.add('oculto');
    }
    target.classList.remove('oculto', 'fade-out');
    target.classList.add('visible');
    updateNumeroPagina(paginaActual);
  }

  if (current) {
    current.classList.remove('visible');
    current.classList.add('fade-out');
    current.addEventListener('animationend', function () {
      showTarget();
    }, { once: true });
  } else {
    showTarget();
  }
}

function retrocederPagina() {
  let pagina = getNumeroPagina();
  if (pagina > 1) {
    showPagina(pagina - 1);
  }
}

function avanzarPagina() {
  const paginas = document.querySelectorAll('section[id^="pag"]');
  const totalPaginas = paginas.length;
  let pagina = getNumeroPagina();
  if (pagina < totalPaginas) {
    showPagina(pagina + 1);
  }
}

window.addEventListener("DOMContentLoaded", function () {
  showPagina(1);
});   
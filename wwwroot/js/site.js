function mostrarContenido() {
  const contenido = document.getElementById("contenido");
  const sobre = document.getElementById("sobre");
  contenido.classList.remove("oculto");
  sobre.classList.add("oculto");
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
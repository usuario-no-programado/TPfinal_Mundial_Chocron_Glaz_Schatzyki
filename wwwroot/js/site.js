function mostrarContenido() {
  const contenido = document.getElementById("contenido");
  const sobre = document.getElementById("sobre")
  contenido.classList.remove("oculto");
  sobre.classList.add("oculto");
}

function retrocederPagina() {
  const numeroPagina = document.getElementById("numeroPagina");
  let pagina = parseInt(numeroPagina.textContent.split(" ")[1]);
  if (pagina > 1) {
    pagina--;
    numeroPagina.textContent = `Pagina ` + pagina;
  }
}

function avanzarPagina() {
  const numeroPagina = document.getElementById("numeroPagina");
  let pagina = parseInt(numeroPagina.textContent.split(" ")[1]);
  pagina++;
  numeroPagina.textContent = `Pagina ` + pagina;
}   
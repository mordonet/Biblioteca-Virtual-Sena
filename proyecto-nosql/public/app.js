// Cargar todos los productos
async function cargarProductos() {
  const res = await fetch("tienda.productos.json");
  const data = await res.json();
  const lista = document.getElementById("lista");
  lista.innerHTML = "";
  data.forEach(p => {
    const li = document.createElement("li");
    li.textContent = `${p.nombre} - $${p.precio} - Stock: ${p.stock}`;
    lista.appendChild(li);
  });
}

// Filtrar productos por categoría
function filtrarPorCategoria(cat) {
  fetch("tienda.productos.json")
    .then(r => r.json())
    .then(data => {
      const filtrados = data.filter(p => p.categoria === cat);
      const lista = document.getElementById("lista");
      lista.innerHTML = "";
      filtrados.forEach(p => {
        const li = document.createElement("li");
        li.textContent = `${p.nombre} - $${p.precio}`;
        lista.appendChild(li);
      });
    });
}

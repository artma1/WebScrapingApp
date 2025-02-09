$(document).ready(function () {
  if (window.location.pathname === '/WebScrapingResults/Edit') {
    getEdited();
    debugger;
  }
});

function getEdited() {

  $.ajax({
    url: '/WebScrapingResults/Edit', 
    type: 'GET',
    success: function (data) {
      $("#editContent").html(data);
    },
    error: function () {
      alert("Erro ao carregar os dados");
    }
  });
}
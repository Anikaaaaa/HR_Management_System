$(document).ready(function () {
   var deptSelect = "All";
   var httpSearchAndSortRequest = new XMLHttpRequest();
   var httpDeleteRequest = new XMLHttpRequest();
   var srch = "";

   $(document).on("change", "#dept_select", function () {
      deptSelect = $("#dept_select").val();
      SelectAndSearch();
   });

   $("#searchbar").keyup(function () {
      if ($(this).val() != null) {
         srch = $(this).val();
      }
      SelectAndSearch();
   });

   function SelectAndSearch() {
      let url = "https://localhost:7025/Employee/SearchAndSort"
      var data = "srch=" + srch + "&dept=" + deptSelect;
      makeSearchAndSortRequest(url, data);
   }

   function makeSearchAndSortRequest(url, data) {
      httpSearchAndSortRequest.onreadystatechange = alertSearchSortContents;
      httpSearchAndSortRequest.open('POST', url, false);
      httpSearchAndSortRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      httpSearchAndSortRequest.send(data);
   }

   function alertSearchSortContents() {
      if (httpSearchAndSortRequest.readyState === XMLHttpRequest.DONE) {
         if (httpSearchAndSortRequest.status === 200) {
            $("#emp_table").html(this.response)
            $("#dept_select").val(deptSelect)
         } else {
            alert("There was a problem with the request.");
         }
      }
   }

   $(document).on("click", ".delete", function () {
      let confirmation = confirm("Are you sure to delete this employee?");

      if (confirmation) {
         let id = this.id;
         let url = "https://localhost:7025/Employee/Delete";
         let data = "id=" + id + "&srch=" + srch + "&dept=" + deptSelect;
         makeDeleteRequest(url, data);
      }
   });

   function makeDeleteRequest(url, data) {
      httpDeleteRequest.onreadystatechange = alertDeleteContents;
      httpDeleteRequest.open('POST', url, false);
      httpDeleteRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      httpDeleteRequest.send(data);
   }

   function alertDeleteContents() {
      if (httpDeleteRequest.readyState === XMLHttpRequest.DONE) {
         if (httpDeleteRequest.status === 200) {
            $("#emp_table").html(this.response)
            $("#dept_select").val(deptSelect)
            var x = document.getElementById("snackbar");
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 800);
         } else {
            alert("There was a problem with the request.");
         }
      }
   }
});
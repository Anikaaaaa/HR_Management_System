$(document).ready(function () {

   var httpSearchRequest = new XMLHttpRequest();
   var httpGetEmpCount = new XMLHttpRequest();
   var httpDeleteDeptRequest = new XMLHttpRequest();
   var srchDept = "";
   var employeeID;

   $("#searchbar_dept").keyup(function () {
      if ($(this).val() != null) {
         srchDept = $(this).val();
      }
      SearchDept();
   });

   function SearchDept() {
      let url = "https://localhost:7025/Department/Search"
      var data = "srch=" + srchDept;
      makeSearchRequest(url, data);
   }

   function makeSearchRequest(url, data) {
      httpSearchRequest.onreadystatechange = alertSearchContents;
      httpSearchRequest.open('POST', url, false);
      httpSearchRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      httpSearchRequest.send(data);
   }

   function alertSearchContents() {
      if (httpSearchRequest.readyState === XMLHttpRequest.DONE) {
         if (httpSearchRequest.status === 200) {
            $("#dept_table").html(this.response)
         } else {
            alert("There was a problem with the request.");
         }
      }
   }

   function GetEmpCount(id) {
      let url = "https://localhost:7025/Department/getEmpCount?id=" + id;
      httpGetEmpCount.onreadystatechange = DeleteDept;
      httpGetEmpCount.open('GET', url);
      httpGetEmpCount.send();
   }

   $(document).on("click", ".deletedpt", function () {
      let confirmation = confirm("Are you sure to delete this department?");

      if (confirmation) {
         employeeID = this.id;
         GetEmpCount(employeeID);
      }
   });

   function DeleteDept() {
      if (httpGetEmpCount.readyState === XMLHttpRequest.DONE) {
         if (httpGetEmpCount.status === 200) {
            let empCount = this.responseText;
            if (empCount > 0) {
               alert("Can not delete this department. This depatrment is having employee/employees.")
            }
            else {
               deleteDepartment();
            }
         }
         else {
            alert("Something is wrong with this request");
         }
      }
   }

   function deleteDepartment() {
      let url = "https://localhost:7025/Department/Delete";
      let data = "id=" + employeeID + "&srch=" + srchDept;
      makeDeleteDeptRequest(url, data);
   }

   function makeDeleteDeptRequest(url, data) {
      httpDeleteDeptRequest.onreadystatechange = alertDeleteDeptContents;
      httpDeleteDeptRequest.open('POST', url, false);
      httpDeleteDeptRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      httpDeleteDeptRequest.send(data);
   }

   function alertDeleteDeptContents() {
      if (httpDeleteDeptRequest.readyState === XMLHttpRequest.DONE) {
         if (httpDeleteDeptRequest.status === 200) {
            $("#dept_table").html(this.response)
            var x = document.getElementById("snackbar2");
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 800);
         } else {
            alert("There was a problem with the request.");
         }
      }
   }
});
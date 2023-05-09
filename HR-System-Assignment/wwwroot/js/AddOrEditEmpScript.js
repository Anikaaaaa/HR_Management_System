$(document).ready(function () {
   var srcData;

   $(function () {
      $("#my_date_picker").datepicker({
         endDate: '+0d',
         autoclose: true
      });
   });

   $("#img").change(function (event) {
      let reader = new FileReader();
      let f = event.target.files[0];

      reader.onloadend = function () {
         srcData = reader.result
         $("#loaded_image").attr("src", srcData);
         $("#img").hide();
         console.log(srcData);
         $("#picked_image").show();
      }
      reader.readAsDataURL(f);
   });

   $("#img1").change(function (event) {
      let reader = new FileReader();
      let f = event.target.files[0];

      reader.onloadend = function () {
         srcData = reader.result
         $("#loaded_image1").attr("src", srcData);
         $("#img1").hide();
         console.log(srcData);
         $("#picked_image1").show();
      }
      reader.readAsDataURL(f);
   });

   $("#choose_again").click(function () {
      ChooseAgain();
   });

   $("#choose_again1").click(function () {
      ChooseAgain1();
   });

   function ChooseAgain() {
      $("#img").show();
      $("#img").val(null);
      srcData = null;
      $("#photo").val(null);
      $("#picked_image").hide();
      $("#photolink").val(null);
   }

   function ChooseAgain1() {
      $("#img1").show();
      $("#img1").val(null);
      srcData = null;
      $("#photo").val(null);
      $("#picked_image1").hide();
      $("#photolink1").val(null);
   }
});
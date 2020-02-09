$(document).ready(function () {
    dateDefaultValues = function () {
        year = @DateTime.Now.Year;
        month = @DateTime.Now.Month;
        if (month < 10)
            month = '0' + month;
        day = @DateTime.Now.Day;
        if (day < 10)
            day = '0' + day;
        date = year + "-" + month + "-" + day;

        $("#dateNow").val(date);

        date = (year - 1) + "-" + month + "-" + day;
        $("#fromDate").val(date);
    }
    dateDefaultValues();

    $("#category").change(function () {
        $("#hiddenMainCategory").val($("#category").val());
        $("#categoryForm").submit();
    });
    var theAllOption = ($("<option>הכל</option>"));
    $("#categoryForm").submit(function (event) {
        event.preventDefault();
        var target = $(this);

        $.ajax({
            url: target.attr("action"),
            data: target.serialize(),
            success: function (data) {

                $("#subCategories").empty().append(theAllOption);
                $.each(data, function (i, value) {
                    $("#subCategories").append($("<option>" + JSON.stringify(value).split('"')[1] + "</option>"));
                });
            }


        });

    });
    $(function () {
        $("table tr.view").on("click", function () {
            $(this).next(".fold").toggleClass("active");
        });
    })
});
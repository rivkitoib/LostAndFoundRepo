var i;
var j;
var res;
var hint = 0;
var v = 4;
var flag = 1;
var seconds;
var m = 0;
$(document).ready(function () {
    $("#showExc").text(" ");
    ///////////////////////////////////////////////////////סליידר לבחירת הזמן
    var MYslider = $("<div></div>").addClass("slider").slider({
        min: 2,
        max: 15,
        value: 4,

        slide: function (event, ui) {

            $(".slider").children("span").text(ui.value);
            v = ui.value;

        }
    }).appendTo("#tabs");
    $(".slider").children("span").text($(".slider").slider("value"));
    for (i = 0; i <= 10; i++) {
        var row = $("<tr></tr>")
        $("#MyTable").append(row)
        for (j = 0; j <= 10; j++) {
            var cell = $("<td></td>")
            if (i != 0 && j != 0)
                cell.text(i * j)
            else {
                if (i == 0)
                    cell.text(j)
                if (j == 0)
                    cell.text(i)
                cell.addClass("first")
            }
            $(row).append(cell)
        }
        $("#MyTable").append(row)
    }

    for (var i = 2; i <= 10; i++) {
        var b = $("<button></button>").attr("id", i);
        b.text("כפולות" + i);
        $("#buttons").append(b);
    }
    var num1;
    var num2;
    $(function () {
        ///////////////////////////////////////////////////////בעת לחיצה על הלשוניות הכל נעצר
        $("#tabs").tabs({
            activate: function (event, ui) {
                flag = 0;
            }


        });
    });
    $("[href='#excercise']").click(function () {
        location.reload();
    })
    $("#excercise").append($('<input/>').attr({ type: 'button', name: 'newExc', value: 'תרגיל חדש' }));
    $("#excercise").append($('<div><div/>').attr("id", "showExc"));
    $("#excercise").append($('<div><div/>').attr("id", "result"));
    $("[name='newExc']").click(function () {
        m = 0;
        seconds = setInterval(countSeconds, 1000);
        function countSeconds() {
            m++;
        }
        flag = 1;
        hint = 0;
        num1 = 1 + Math.floor(Math.random() * 10);
        num2 = 1 + Math.floor(Math.random() * 10);
        res = num1 * num2;
        $("#showExc").text(num1 + 'x' + num2);
        $("#result").text(" ");
        $("#result").css("backGround", "white");
        ///////////////////////////////////////////////////////גרירה ושחרור
        $('td').draggable({
            revert: "invalid",
            stack: ".draggable",
            helper: 'clone'
        });

        $("#result").droppable({
            drop: function (event, ui) {
                if (ui.draggable["0"].innerHTML == res) {
                    flag = 0;
                    $("#dialog2").append('<p></p>>').text("You succeded after " + hint + " hints" + " in " + m + " seconds");
                    $("#dialog2").dialog({});
                    setTimeout(function () {
                        $("#dialog2").dialog("close");
                        $("[name='newExc']").trigger("click");
                    }, 2000);
                    $(this).text(res);
                    $(this).css("backgroundColor", "#abf8ff");


                }
            }
        });
        ///////////////////////////////////////////////////////בעת לחיצה על הלשוניות הכל נעצר
        $(function prog() {

            var progressbar = $("#progressbar"),
                progressLabel = $(".progress-label");

            progressbar.progressbar({
                value: false,
                change: function () {
                    progressLabel.text(progressbar.progressbar("value") + "%");
                },
                complete: function () {
                    if (hint < 3) {
                        ++hint;
                        if (hint == 1) {
                            rowHint();
                            prog();
                        }
                        if (hint == 2) {
                            cellHint()
                            prog();

                        }
                        if (hint == 3) {//דיאלוג כשלון
                            $("#dialog").dialog();
                            $("#dialog").text("Try again!");
                            $("#dialog").dialog({
                                buttons: [{
                                    id: "ans", "data-test": "data test", text: "הצג תשובה",
                                    click: function () {
                                        $(this).dialog("close");
                                        HighLight($("tr").eq(num1).children(0).eq(num2));
                                    }

                                }, {
                                    id: "stud", "data-test": "data test", text: "למידת כפל",
                                    click:
                                    function () {
                                        $(this).dialog("close");
                                        $("#tabs").tabs({ active: 1 });
                                    }
                                }]
                            });
                        }
                    }

                }
            });

            function progress() {
                var val = progressbar.progressbar("value") || 0;

                progressbar.progressbar("value", val + v);

                if (val < 99 && flag) {
                    setTimeout(progress, 500);
                }
            }

            setTimeout(progress, 2000);
        });
    });

    function rowHint() {
        HighLight($("tr").eq(num1).children(0).not($("tr").eq(num1).children(0).first()));
        HighLightFirst($("tr").eq(num1).children(0).first());
    }

    function cellHint() {
        HighLight($("tr").eq(num1).children(0).eq(num2));
        if (num2 == 1)
            HighLight($("tr").eq(num1).children(0).eq(num2 + 2));
        else
            HighLight($("tr").eq(num1).children(0).eq(num2 - 1));
        if (num2 == 10)
            HighLight($("tr").eq(num1).children(0).eq(num2 - 2));
        else
            HighLight($("tr").eq(num1).children(0).eq(num2 + 1));
    }

    $("button").click(function () {
        for (var item in $("button")) {
            $(item).prop('disabled', true);
        }

        abc(1, this)
    });

    function abc(i, t) {
        var c = $(t).attr('id');
        HighLightFirst($("td").eq(c));
        setTimeout(function () {
            HighLightFirst($("tr").eq(i).children(0).first())
        }, 250*(10-v));
        setTimeout(function () {
            HighLight($("tr").eq(i).children(0).eq(c));
        }, 500*(10-v));
        if (i < 10)
            setTimeout(function () {
                abc(++i, t)
            }, 750*(10-v))
        else
            for (var item in $("button")) {
                $(item).prop('disabled', false);
            }
    }

    function HighLight(item) {

        $(item).animate({
            backgroundColor: "#226b71",
            color: "#fff",
        }, 500);
        $(item).animate({
            backgroundColor: "#abf8ff",
            color: "#000",
        }, 1000);
    };

    function HighLightFirst(item) {

        $(item).animate({
            backgroundColor: "#226b71",
            color: "#fff",
        }, 500);
        $(item).animate({
            backgroundColor: "#79ced6",
            color: "#000",
        }, 1000);
    };

});





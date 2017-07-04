function ShowDiagram(tags) {

    var chart = new CanvasJS.Chart("chartContainer", {
        title: {
            text: "Top 10 Tags"
        },

        theme: "theme2",
        animationEnabled: true,

        data: [
        {
            type: "pie",
            dataPoints: [
{ label: tags[0].Name, y: tags[0].Count },
{ label: tags[1].Name, y: tags[1].Count },
{ label: tags[2].Name, y: tags[2].Count },
{ label: tags[3].Name, y: tags[3].Count },
{ label: tags[4].Name, y: tags[4].Count },
{ label: tags[5].Name, y: tags[5].Count },
{ label: tags[6].Name, y: tags[6].Count },
{ label: tags[7].Name, y: tags[7].Count },
{ label: tags[8].Name, y: tags[8].Count },
{ label: tags[9].Name, y: tags[9].Count },
            ],

        }

        ]
    });

    chart.render();
}

function GetTags() {
    $.ajax({
        type: 'GET',
        beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', 'Bearer ' + appContext.token); },
        url: appContext.buildUrl('/api/tags/getTop10'),
        dataType: "json",
        success: function (tags) {
            ShowDiagram(tags);
        }
    });
}

function Show_Hide() {

    if ($('.topTags').css('display') == 'none') {
        GetTags();
        $('.topTags').show();
        $('.notes-board').hide();
    }
    else {
        $('.topTags').hide();
        $('.notes-board').show();
    }
}

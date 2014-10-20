$(document).ready(function () {

    $(".imgLoading").show();

    var url = "http://localhost:9000/api/kpis/getall?userId=valtea00",
        kpi = [];
   
    var retrieveData = function () {
        var result = arguments[0],
            AC = parseFloat(normalizeData(result.AnnoCorrenteAgenzia)),
            AP = parseFloat(normalizeData(result.AnnoPrecedenteAgenzia)),
            PD = parseFloat(normalizeData(result.PariDataAgenzia)),

            AcMese = parseFloat(normalizeData(result.AnnoCorrenteAgenziaMese)),
            ApMese = parseFloat(normalizeData(result.AnnoPrecedenteAgenziaMese)),
            PdMese = parseFloat(normalizeData(result.PariDataAgenziaMese)),

            budget = 140000000;

        
        bindChart("chartdiv",AC,AP,PD,budget);
        bindChart("chartdiv2", AcMese,ApMese,PdMese, budget);

        

        $("a[title='JavaScript charts']").hide();
        $(".imgLoading").hide();

    };

    $.when($.ajax(url))
        .then(retrieveData);
});


function bindChart(divID, AnnoCorrente, AnnoPrecedente, PariData, Budget){
    AmCharts.makeChart(divID, {
            "type": "gauge",
              
            "axes": [{
                "axisThickness":1,
                 "axisAlpha":0.2,
                 "tickAlpha":0.2,
                 "valueInterval":20000000,
                 "usePrefixes":true,
                "bands": [{
                    "color": "#84b761",
                    "endValue": AnnoCorrente,
                    "startValue": 0,
                    "balloonText": "Anno corrente &euro; "+deNormalizeData(AnnoCorrente),
                    "innerRadius": "95%",
                }, {
                    "color": "#cc4748",
                    "endValue": PariData,
                    "startValue": AnnoCorrente,
                    "balloonText": "Pari data &euro; "+deNormalizeData(PariData),
                    "innerRadius": "95%",
                }, {
                    "color": "#fdd400",
                    "endValue": AnnoPrecedente,
                    "startValue": PariData,
                    "balloonText": "Anno precedente &euro; "+deNormalizeData(AnnoPrecedente),
                    "innerRadius": "95%",
                }, {
                    "color": "#207CE5",
                    "endValue": Budget,
                    "startValue": AnnoPrecedente,
                    "balloonText": "Budget &euro; "+deNormalizeData(Budget),
                    "innerRadius": "95%",
                }],
                "bottomText": deNormalizeData(AnnoCorrente),
                "bottomTextYOffset": -20,
                "endValue": Budget
            }],
           "arrows": [
                {
                    "value": AnnoCorrente,
                    "color":"#000"
                }
            ]
        });
}

function deNormalizeData(strNumber){
    return numeral(strNumber).format('0,0.00').replace(/,/g, '_').replace('.',',').replace(/_/g, ',')
};

function normalizeData(strNumber){
    return strNumber;
};



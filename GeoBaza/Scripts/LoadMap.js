$(document).ready(function () {
   

    debugger;
    //var locations =
        $.ajax({

        type: "GET",

        url: "Home/GetJsonLocation", 

       success: loadMaps

   });
    debugger;

    //var rivers = $.ajax({

    //    type: "GET",

    //    url: "Home/GetJsonRivers",

    //    success: getRivers

    //});

    //loadMaps(locations);

        
});

function getLocations(data) {
    debugger;
    var lokacije = data;
    return lokacije;

}

function getRivers(data) {
    debugger;
    var rivers = data;
    return lokacije;
}


function loadMaps(data) {

    var lokacije = data;
   
    var stroke = new ol.style.Stroke({ color: 'black', width: 12 });
    var fill = new ol.style.Fill({ color: 'blue' });

    var lokacijeStyle = new ol.style.Style({
        image: new ol.style.RegularShape({
            fill: fill,
            stroke: stroke,
            points: 4,
            radius: 10,
            angle: Math.PI / 4
        }),
        text: new ol.style.Text({
            font: 'bold 11px "Open Sans", "Arial Unicode MS", "sans-serif"',
            placement: 'point',
            fill: new ol.style.Fill({
                color: 'black'
            })
        })
    });

   

    //var putevi = @Html.Raw(Model.roads);

    debugger;
   
    //var rekeSource = new ol.source.Vector({
    //    features: (new ol.format.GeoJSON({
    //        dataProjection: 'EPSG:4326',
    //        featureProjection: 'EPSG:4326'
    //    })).readFeatures(reke)
    //});

    //var rekeLayer = new ol.layer.Vector({
    //    source: rekeSource,
    //    style: new ol.style.Style({
    //        stroke: new Stroke({
    //            color: 'blue',
    //            width: 1
    //        })
    //    }),
    //});

    //var puteviSource = new ol.source.VectorSource({
    //    features: (new ol.format.GeoJSON()).readFeatures(putevi)
    //});
    //puteviLayer = new ol.layer.VectorLayer({
    //    source: puteviSource,
    //    style: new ol.style.Style({
    //        stroke: new Stroke({
    //            color: 'orange',
    //            width: 1
    //        })
    //    }),
    //});

    var lokacijeSource = new ol.source.Vector({
        features: (new ol.format.GeoJSON({
            dataProjection: 'EPSG:4326',
            featureProjection: 'EPSG:4326'
        })).readFeatures(lokacije)
    });

    var lokacijeLayer = new ol.layer.Vector({
        source: lokacijeSource,
        style: function (feature) {
            lokacijeStyle.getText().setText(feature.get('name'));
            return lokacijeStyle;
        }
});

    var map = new ol.Map({
        target: 'map',
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            }),
            //rekeLayer,
            lokacijeLayer
        ],
        view: new ol.View({
            projection: 'EPSG:4326',
            center: [19.8, 45.2],
            zoom: 10
        })
    });





    map.on('pointermove', showInfo);

    var info = document.getElementById('info');

    function showInfo(event) {
        var features = map.getFeaturesAtPixel(event.pixel);
        if (!features) {
            info.innerText = '';
            info.style.opacity = 0;
            return;
        }
        var properties = features[0].getProperties();
        delete properties.geometry;
        info.innerText = JSON.stringify(properties, null, 2);
        info.style.opacity = 1;
    }

}
$("#service").change(function () {
    $("#submitService").trigger("click");
    $("#request").html("");
});

$("#sampleMessage").click(function () {
    var oiid;
    var random = Math.random() * 4;
    if (random < 1)
        oiid = "40254B02B4";
    else if (random < 2)
        oiid = "1DFFC02B78";
    else if (random < 3)
        oiid = "5C99BAF52A";
    else if (random < 4)
        oiid = "A793B021DD";
    $("#immunizationId").val(oiid);
});

$("#pccdrSampleMessage").click(function () {
    $("#healthCardNumber").val("8888-888-888-BB");
    if ($("#resourceType").val() === "Observation") {
        $("#category").val("vital-signs");
        $("#code").val("");
        $("#code").attr("disabled", true);
    }
});

$("#dhdrSampleMessage").click(function () {
    var random = Math.random() * 4;
    if ($("#queryType").val() === "1-Point") {
        OnePointQuery(random);
    }
    else {
        ThreePointQuery(random);
    }
});

OnePointQuery = function (random) {
    var hcn;
    var date;
    if (random < 1) {
        hcn = "6948425589";
        date = "2018-02-24";
    }
    else if (random < 2) {
        hcn = "6408383104";
        date = "2018-03-19";
    }
    else if (random < 3) {
        hcn = "8028261884";
        date = "2018-03-17";
    }
    else if (random < 4) {
        hcn = "2900000684";
        date = "2018-03-15";
    }
    $("#healthCardNumber").val(hcn);
    $("#dateDispensed").val(date);
};

ThreePointQuery = function (random) {
    var hcn;
    var date;
    var dob;
    var gender;
    if (random < 1) {
        hcn = "6948425589";
        date = "2018-02-24";
        gender = "female";
        dob = "1994-01-15";
    }
    else if (random < 2) {
        hcn = "6408383104";
        date = "2018-03-19";
        gender = "male";
        dob = "1967-09-05";
    }
    else if (random < 3) {
        hcn = "8028261884";
        date = "2018-03-17";
        gender = "female";
        dob = "2008-02-09";
    }
    else if (random < 4) {
        hcn = "2900000684";
        date = "2018-03-15";
        gender = "female";
        dob = "1940-06-12";
    }
    $("#healthCardNumber").val(hcn);
    $("#dateDispensed").val(date);
    $("#gender").val(gender);
    $("#dateOfBirth").val(dob);
};

$("#toggleDhdr").click(function () {
    var current = $("#toggleDhdr").html();
    if (current === "1-Point") {
        $("#toggleDhdr").removeClass().addClass("btn btn-warning");
        $("#toggleDhdr").html("3-Point");
        $("#dateOfBirth").removeAttr("disabled");
        $("#gender").removeAttr("disabled");
        $("#dateOfBirth").attr("required", true).val("");
        $("#gender").attr("required", true).val("");
        $("#queryType").val("3-Point");
    }
    else {
        $("#toggleDhdr").removeClass().addClass("btn btn-primary");
        $("#toggleDhdr").html("1-Point");
        $("#dateOfBirth").removeAttr("required");
        $("#gender").removeAttr("required");
        $("#dateOfBirth").attr("disabled", true).val("");
        $("#gender").attr("disabled", true).val("");
        $("#queryType").val("1-Point");
    }
});


$("#resourceType").change(function () {
    var resource = $("#resourceType").val();

    //Clear values
    $("#date").val("");
    $("#dateModifier").val("");
    $("#code").val("");
    $("#category").val("");
    $("#clinicalStatus").val("");
    $("#includeMedication").prop('checked',false);
    $("#healthCardNumber").val("");

    //Base Query
    $("#dateSearch").attr("hidden", true);
    $("#codeSearch").attr("hidden", true);
    $("#categorySearch").attr("hidden", true);
    $("#clinicalStatusSearch").attr("hidden", true);
    $("#medicationSearch").attr("hidden", true);
    $("#category").removeAttr("disabled");
    $("#code").removeAttr("disabled");

    if (resource === "Procedure")
        $("#dateSearch").removeAttr("hidden");
    else if (resource === "MedicationRequest")
        $("#medicationSearch").removeAttr("hidden");
    else if (resource === "Condition") {
        $("#categorySearch").removeAttr("hidden");
        $("#clinicalStatusSearch").removeAttr("hidden");
    }
    else if (resource === "Observation") {
        $("#dateSearch").removeAttr("hidden");
        $("#codeSearch").removeAttr("hidden");
        $("#categorySearch").removeAttr("hidden");
    }
});


$("#category").keyup(function () {
    if ($("#category").val() != "")
        $("#code").attr("disabled", true);
    else
        $("#code").removeAttr("disabled");
});

$("#code").keyup(function () {
    if ($("#code").val() != "")
        $("#category").attr("disabled", true);
    else
        $("#category").removeAttr("disabled");
});
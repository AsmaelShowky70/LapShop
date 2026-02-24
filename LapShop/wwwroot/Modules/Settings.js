var ClsSettings = {
    GetAll: function () {
        Helper.AjaxCallGet("https://lapshope.runasp.net/api/Setting", {}, "json",
            function (data) {
                console.log(data);
                $("#lnkFacebook").attr("href", data.facebookLink);
            }, function () { });
    }
}

ClsSettings.GetAll();

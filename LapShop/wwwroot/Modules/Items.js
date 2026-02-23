
var ClsItems = {
    GetAll: function () {
        var params = new URLSearchParams(window.location.search);
        var categoryId = params.get("categoryId");
        var apiUrl = "/api/Items";
        if (categoryId) {
            apiUrl = "/api/Items/GetByCategoryId/" + categoryId;
        }

        Helper.AjaxCallGet(apiUrl, {}, "json",
            function (data) {
                var allItems = data.data || [];
                var q = params.get("q");
                if (q) {
                    var term = q.toLowerCase();
                    allItems = allItems.filter(function (item) {
                        return item.itemName && item.itemName.toLowerCase().indexOf(term) !== -1;
                    });
                }


                $('#ItemPagination').pagination({
                    dataSource: allItems,
                    pageSize: 20,
                    showGoInput: true,
                    showGoButton: true,
                    callback: function (data, pagination) {
                        // template method of yourself
                        var htmlData = "";

                        for (var i = 0; i < data.length; i++) {
                            htmlData += ClsItems.DrawItem(data[i]);
                        }

                        var d1 = document.getElementById('ItemArea');
                        d1.innerHTML = htmlData;
                    }
                });
            }, function () { });
    },
    DrawItem: function (item) {
        var data = "<div class='col-xl-3 col-6 col-grid-box'>";
        var detailsUrl = "/Items/ItemDetails/" + item.itemId;
        var imageUrl = "/Uploads/Items/" + item.imageName;
        var itemName = item.itemName || "";

        data += "<div class='product-box'><div class='img-wrapper'>";
        data += "<div class='front'><a href='" + detailsUrl + "'><img src='" + imageUrl + "' class='img-fluid blur-up lazyload bg-img' alt='" + itemName + "'></a></div>";
        data += "<div class='back'><a href='" + detailsUrl + "'><img src='" + imageUrl + "' class='img-fluid blur-up lazyload bg-img' alt='" + itemName + "'></a></div>";
        data += "<div class='cart-info cart-wrap'>";
        data += "<a href='/Order/AddToCart?itemId=" + item.itemId + "' title='Add to cart'><i class='ti-shopping-cart'></i></a>";
        data += "<a href='javascript: void (0)' title='Add to Wishlist'><i class='ti-heart' aria-hidden='true'></i></a>";
        data += "</div></div>";
        data += "<div class='product-detail'><div class='rating'> <i class='fa fa-star'></i> <i class='fa fa-star'></i> <i class='fa fa-star'></i>";
        data += "<i class='fa fa-star'></i> <i class='fa fa-star'></i></div>";
        data += "<a href='" + detailsUrl + "'><h6>" + itemName + "</h6></a><p> </p>";
        data += "<h4>$" + item.salesPrice + "</h4>";
        data += "<ul class='color-variant'><li class='bg-light0'></li><li class='bg-light1'></li><li class='bg-light2'></li></ul> </div> </div> </div>";
        return data;
    }
}


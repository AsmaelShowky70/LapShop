
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
        var isArabic = document.documentElement.lang === 'ar';
        var detailsUrl = "/Items/ItemDetails/" + item.itemId;
        var imageUrl = "/Uploads/Items/" + item.imageName;
        var itemName = item.itemName || "";
        var salesPrice = item.salesPrice;

        // Date parsing for "New" badge (simplified)
        var isNew = false;
        if (item.createdDate) {
            var createdDate = new Date(item.createdDate);
            var thirtyDaysAgo = new Date();
            thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
            if (createdDate >= thirtyDaysAgo) {
                isNew = true;
            }
        }

        var html = '<div class="col-xl-3 col-6 col-grid-box mb-4">';
        html += '<div class="product-card h-100">';
        html += '<div class="img-wrapper">';

        if (isNew) {
            html += '<span class="badge-custom badge-new">' + (isArabic ? "جديد" : "New") + '</span>';
        }

        html += '<a href="' + detailsUrl + '">';
        html += '<img src="' + imageUrl + '" alt="' + itemName + '" loading="lazy">';
        html += '</a>';
        html += '</div>';

        html += '<div class="product-detail">';
        html += '<a href="' + detailsUrl + '" class="text-decoration-none">';
        html += '<h6>' + itemName + '</h6>';
        html += '</a>';

        html += '<div class="d-flex justify-content-between align-items-center mt-2 mb-3">';
        html += '<span class="price">' + salesPrice + ' ' + (isArabic ? "ج.م" : "EGP") + '</span>';
        html += '<div class="rating small text-warning">';
        html += '<i class="fa fa-star"></i> <i class="fa fa-star"></i> <i class="fa fa-star"></i> <i class="fa fa-star"></i> <i class="fa fa-star"></i>';
        html += '</div>';
        html += '</div>';

        html += '<a href="/Order/AddToCart?itemId=' + item.itemId + '" class="btn-add-to-cart">';
        html += '<i class="fa fa-shopping-cart"></i> ';
        html += (isArabic ? "أضف للسلة" : "Add to Cart");
        html += '</a>';

        html += '</div>'; // End product-detail
        html += '</div>'; // End product-card
        html += '</div>'; // End col

        return html;
    }
}

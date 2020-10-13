function GetCategoriesFromAPI() {
    console.log("loading...");
    $.ajax({
        type: "GET",
        url: `https://taco-food-api.herokuapp.com/api/v1/category`,
        success: function (data) {
            let categories = [];

            data.forEach(item => {
                let category = {
                    Id: 0,
                    Name: item.category,
                }
                categories.push(category);
            });

            $.ajax({
                type: "PUT",
                data: { categories: categories },
                url: "/Category/Create",
                success: function (data) {
                    if (data.success) {
                        console.log(data.message);
                    }
                    else {
                        console.error(data.message);
                    }
                }
            });
        }
    });
}

function GetProductsFromAPI() {
    console.log("loading...");
    $.ajax({
        type: "GET",
        url: `https://taco-food-api.herokuapp.com/api/v1/food`,
        success: function (data) {
            let products = [];

            data.forEach(item => {
                let product = {
                    Id: 0,
                    IdCategory: item.category_id,
                    Name: item.description,
                    BaseQty: item.base_qty,
                    BaseUnit: item.base_unit,
                }
                products.push(product);
            });

            products = JSON.stringify(products);

            $.ajax({
                type: "PUT",
                data: { productsJson: products },
                url: "/Product/Create",
                success: function (data) {
                    if (data.success) {
                        console.log(data.message);
                    }
                    else {
                        console.error(data.message);
                    }
                }
            });
        }
    });
}

function Delete(idMarketList) {
    Swal.fire({
        title: 'Tem certeza?',
        text: "Após confirmar não será possível recuperar!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#ee8700',
        cancelButtonColor: '#c5c5c5',
        confirmButtonText: 'Apagar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: "/MarketList/Delete",
                data: { id: idMarketList },
                success: function (data) {
                    if (data.success) {
                        window.location.href = "/MarketList";
                    }
                    else {
                        console.log(data.message);
                    }
                }
            });
        }
    });
}
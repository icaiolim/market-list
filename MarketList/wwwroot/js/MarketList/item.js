function Delete(idProductList) {
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
                url: "/ProductList/Delete",
                data: { idProductList: idProductList },
                success: function (data) {
                    if (data.success) {
                        window.location.href = `/MarketList/Item/${data.idMarketList}`;
                    }
                    else {
                        console.log(data.message);
                    }
                }
            });
        }
    });
}

function CheckItem(idProductList) {
    $.ajax({
        type: "PUT",
        url: "/ProductList/CheckItem",
        data: { idProductList: idProductList },
        success: function (data) {
            if (data.success) {
                $(`table tr[data-id=${idProductList}]`).toggleClass("checked");
                $(`.product-list[data-id=${idProductList}]`).toggleClass("checked");
            }
            else {
                console.log(data.message);
            }
        }
    });
}

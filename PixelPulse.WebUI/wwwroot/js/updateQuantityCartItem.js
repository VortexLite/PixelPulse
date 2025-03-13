async function updateQuantityCartItem(input) {
    let cartItemId = input.dataset.cartItemId;
    let newQuantity = input.value;

    if (newQuantity < 1) {
        input.value = 1;
        return;
    }

    let response = await fetch('http://localhost:5144/api/Cart/updateCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ cartItemId: cartItemId, quantity: newQuantity })
    });

    if (!response.ok) {
        console.error("Не вдалося оновити товар у кошику");
    }
}
// Handle add to cart functionality
function addToCart(productId, quantity = 1) {
    // Check if user is authenticated
    const isAuthenticated = document.body.classList.contains('user-authenticated');
    
    if (!isAuthenticated) {
        // Redirect to login page with return URL
        const currentUrl = encodeURIComponent(window.location.href);
        window.location.href = `/Account/Login?returnUrl=${currentUrl}`;
        return;
    }

    // Submit the form to add item to cart
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Cart/AddToCart';

    // Add antiforgery token
    const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
    if (tokenElement) {
        const tokenInput = document.createElement('input');
        tokenInput.type = 'hidden';
        tokenInput.name = '__RequestVerificationToken';
        tokenInput.value = tokenElement.value;
        form.appendChild(tokenInput);
    }

    const productIdInput = document.createElement('input');
    productIdInput.type = 'hidden';
    productIdInput.name = 'productId';
    productIdInput.value = productId;

    const quantityInput = document.createElement('input');
    quantityInput.type = 'hidden';
    quantityInput.name = 'quantity';
    quantityInput.value = quantity;

    form.appendChild(productIdInput);
    form.appendChild(quantityInput);
    document.body.appendChild(form);
    form.submit();
}

// Handle quantity adjustments
document.addEventListener('click', function(event) {
    const target = event.target.closest('.btn-quantity');
    if (!target) return;

    const button = target;
    const cartId = button.dataset.cartId;
    const action = button.dataset.action;
    const input = document.querySelector(`.quantity-input[data-cart-id="${cartId}"]`);
    let currentQuantity = parseInt(input.value);

    if (action === 'decrease' && currentQuantity > 1) {
        currentQuantity--;
    } else if (action === 'increase') {
        currentQuantity++;
    } else if (action === 'decrease' && currentQuantity <= 1) {
        return; // Don't allow quantity below 1
    }

    // Update the input value immediately for better UX
    input.value = currentQuantity;

    // Get the antiforgery token
    const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
    if (!tokenElement) {
        console.error('Antiforgery token not found');
        return;
    }

    // Send update to server
    fetch('/Cart/UpdateQuantity', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': tokenElement.value
        },
        body: JSON.stringify({
            id: parseInt(cartId),
            quantity: currentQuantity
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        if (data.success) {
            // Update the subtotal
            const row = button.closest('tr');
            const subtotalCell = row.querySelector('td:nth-child(4)');
            subtotalCell.textContent = `$${data.subtotal}`;

            // Update the total
            updateCartTotal();
        } else {
            // Revert the quantity if update failed
            input.value = data.quantity || currentQuantity - 1;
            if (data.message) {
                alert(data.message);
            }
        }
    })
    .catch(error => {
        console.error('Error:', error);
        // Revert the quantity
        input.value = currentQuantity - 1;
    });
});

// Helper function to update cart total
function updateCartTotal() {
    const subtotalCells = document.querySelectorAll('td:nth-child(4)');
    const total = Array.from(subtotalCells)
        .reduce((sum, cell) => {
            const value = parseFloat(cell.textContent.replace('$', '')) || 0;
            return sum + value;
        }, 0);
    
    const totalElement = document.querySelector('.cart-total');
    if (totalElement) {
        totalElement.textContent = `$${total.toFixed(2)}`;
    }
}

// Initialize Bootstrap dropdowns
document.addEventListener('DOMContentLoaded', function() {
    // No need for custom dropdown handling as we're using Bootstrap's dropdown
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});

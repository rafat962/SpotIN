function printInvoice() {
    const printable = document.getElementById('printableInvoice');
    if (!printable) {
        alert('Nothing to print.');
        return;
    }

    const receipt = printable.querySelector('.invoice-receipt');
    const contentHtml = receipt ? receipt.outerHTML : printable.innerHTML;

    const printWindow = window.open('', '_blank', 'width=900,height=700');
    if (!printWindow) {
        alert('Please allow pop-ups to print the invoice.');
        return;
    }

    printWindow.document.open();
    printWindow.document.write('<!DOCTYPE html><html lang="en"><head><meta charset="utf-8"><title>Invoice</title>');
    printWindow.document.write('<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">');
    printWindow.document.write('<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">');
    printWindow.document.write('<style>body{padding:24px;color:#212529;background:#fff}@media print{body{padding:0}}</style>');
    printWindow.document.write('</head><body>');
    printWindow.document.write(contentHtml);
    printWindow.document.write('</body></html>');
    printWindow.document.close();

    printWindow.onload = function () {
        printWindow.focus();
        setTimeout(function () {
            printWindow.print();
            printWindow.close();
        }, 300);
    };
}

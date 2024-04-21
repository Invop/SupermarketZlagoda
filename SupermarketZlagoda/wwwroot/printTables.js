function printComponent(htmlContent, tableName) {
    var printWindow = window.open('', '_blank');
    printWindow.document.open();
    printWindow.document.write('<html><head><title>Print</title></head><body>');
    printWindow.document.write('<h2>' + tableName + '</h2>');
    printWindow.document.write(htmlContent);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
}
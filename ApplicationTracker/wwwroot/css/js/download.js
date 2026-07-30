window.downloadFile = function (filename, content) {

    const blob = new Blob(
        [content],
        {
            type: "text/csv;charset=utf-8;"
        }
    );


    const link = document.createElement("a");

    link.href = URL.createObjectURL(blob);

    link.download = filename;

    link.click();

    URL.revokeObjectURL(link.href);
}
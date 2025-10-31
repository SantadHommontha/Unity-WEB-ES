mergeInto(LibraryManager.library, {
    DownloadFile: function (name, base64Data) {
        // แปลง Base64 String ให้เป็น URL Data
        var nameString = UTF8ToString(name);
        var base64String = UTF8ToString(base64Data);
        var dataUrl = "data:image/png;base64," + base64String;

        // สร้าง Link Element สำหรับการดาวน์โหลด
        var link = document.createElement('a');
        link.href = dataUrl;
        link.download = nameString; // กำหนดชื่อไฟล์

        // สั่งคลิก Link เพื่อเริ่มการดาวน์โหลด
        link.click();
        console.log("Screen Shot");
    }
});
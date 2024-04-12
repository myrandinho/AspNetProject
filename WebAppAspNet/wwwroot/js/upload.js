document.addEventListener('DOMContentLoaded', function () {

    handleProfileImageUpload()

})


function handleProfileImageUpload() {
    try {


        let fileUploader = document.querySelector('#uploadFile')

        if (fileUploader != undefined) {
            fileUploader.addEventListener('change', function () {
                if (this.files.length > 0)
                    this.form.submit()
            })
        }


    }
    catch { }
}
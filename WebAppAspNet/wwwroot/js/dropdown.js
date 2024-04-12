
document.addEventListener('DOMContentLoaded', function () {
    select()
    searchQuery()
})




function select() {
    try {
        let select = document.querySelector('.select')
        let selected = select.querySelector('.selected')
        let selectOptions = select.querySelector('.select-options')

        selected.addEventListener('click', function () {
            selectOptions.style.display = (selectOptions.style.display === 'block') ? 'none' : 'block'
        })

        let options = selectOptions.querySelectorAll('.option')

        options.forEach(function (option) {
            option.addEventListener('click', function () {
                selected.innerHTML = this.textContent
                selectOptions.style.display = 'none';
                let category = this.getAttribute('data-value')
                selected.setAttribute('data-value', category)
                updateCourseByFilter()
            })
        })   
    }
    catch { }
}



function searchQuery() {
    try {


        document.querySelector('#searchQuery').addEventListener('keyup', function () {
            updateCourseByFilter()
        })


    }
    catch { }
}




function updateCourseByFilter() {
    const category = document.querySelector('.select .selected').getAttribute('data-value') || 'all'
    const searchQuery = document.querySelector('#searchQuery').value
    


    const url = `/courses?category=${encodeURIComponent(category)}&searchQuery=${encodeURIComponent(searchQuery)}`

    fetch(url)
        .then(res => res.text())
        .then(data => {
            const parser = new DOMParser()
            const dom = parser.parseFromString(data, 'text/html')
            document.querySelector('.items').innerHTML = dom.querySelector('.items').innerHTML

            const pagination = dom.querySelector('.pagination') ? dom.querySelector('.pagination').innerHTML : ''
            document.querySelector('pagination').innerHTML = pagination
        })
}
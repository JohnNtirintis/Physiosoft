function displayError(message, errorElementId) {
    const errorDiv = document.getElementById(errorElementId);
    if (errorDiv) {
        errorDiv.innerText = message;
    }
}

function fetchLastName(type, id, lastNameElementId, errorElementId) {
    fetch(`/${type}/Get${type}LastName?id=` + id)
        .then(response => {
            if (response.ok) {
                return response.text();
            } else {
                throw new Error('Network response was NOT ok.');
            }
        })
        .then(data => {
            if (data) {
                document.getElementById(lastNameElementId).value = data;
            } else {
                throw new Error('No data returned');
            }
        })
        .catch(error => {
            displayError(`Error fetching ${type.toLowerCase()} last name.`, errorElementId);
        });
}

// Fetch last names on page load
fetchLastName('Physios', physioId, 'physioLastName', 'physioError');
fetchLastName('Patients', patientId, 'patientLastName', 'patientError');

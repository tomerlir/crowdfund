// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.
let form = $('.js-form-validations');
form.click(function () {
    if (!form.valid()) {
        return;
    }
});
$("#addMedia").click(function () {
    let html = '';
    html += '<div id="inputFormRow">';
    html += '<div class="input-group mb-3">';
    html += '<input type="text" name="url[]" class="form-control m-input" placeholder="Enter URL">';
    html += '<div class="input-group-append">';
    html += '<button id="removeRow" type="button" class="btn btn-danger">Remove</button>';
    html += '</div>';
    html += '</div>';

    $('#newRow').append(html);
});

$(document).on('click', '#removeRow', function () {
    $(this).closest('#inputFormRow').remove();
});

//--------------------Project Page JS-------------------------//

let backSuccessAlert = $('.js-back-success-alert');
backSuccessAlert.hide();

let backFailAlert = $('.js-back-fail-alert');
backFailAlert.hide();

let rewardAmountButton = $('.js-reward-amount-button');

rewardAmountButton.on('click', (event) => {
    let clickedElement = $(event.currentTarget);
    let rewardAmount = clickedElement.parent().parent().find('.js-button-amount').text();    
    let amountToSet = clickedElement.parent().parent().find('.js-amount');
    amountToSet.val(rewardAmount);
});

let button = $('.js-backit');

button.on('click', (event) => {
    let clickedElement = $(event.currentTarget);
    let amount = clickedElement.parent().parent().find('.js-amount').val();
    let parsedAmount = parseInt(amount);
    let rewardPackageId = clickedElement.parent().parent().find('.js-reward').val();
    let projectId = clickedElement.parent().parent().find('.js-project').val();

    backSuccessAlert.hide();
    backFailAlert.hide();

    let data = {
        "Amount": parseInt(amount),
        "RewardPackageId": parseInt(rewardPackageId),
        "ProjectId": parseInt(projectId)
    };

    clickedElement.parent().parent().find('.js-amount').val('');
 
    $.ajax({
        type: 'POST',
        url: `/projects`,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        let qtyEl = clickedElement.parent().parent().find('.js-qty');
        
        if (parseInt(qtyEl.text()) > 0 && qtyEl.text() !== 'Unlimited') {
            let newQty = parseInt(qtyEl.text()) -1;
            qtyEl.text(newQty);
        }
        
        if (qtyEl.text() !== 'Unlimited' && parseInt(qtyEl.text()) === 0) {
            clickedElement.prop('disabled', true);
        }
        let currentTotal = parseInt(document.querySelector('#total-amount').innerText);
        let goal = parseInt(document.querySelector('#goal').innerText);
        let totalAmount = currentTotal + parsedAmount;
        let percentage = Math.round(totalAmount/goal*100);
        document.querySelector('#percentage').innerText = `${percentage}%`;
        $('.progress-bar').css('width', `${percentage}%`);
        
        document.querySelector('#total-amount').innerText = totalAmount;
        document.querySelector('#backings').innerText = parseInt(document.querySelector('#backings').innerText) + 1;
        $('.js-back-success-modal').modal('show');
             
    }).fail(failureResponse => {
        backFailAlert.text(failureResponse.responseText);
        backFailAlert.show().delay(3000);
        window.scrollTo(0, 0);
        backFailAlert.fadeOut();
    }); 
});                    

//-------------Update User Profile--------------//

let userSuccessAlert = $('.js-user-profile-success-alert');
userSuccessAlert.hide();

let userFailAlert = $('.js-user-profile-fail-alert');
userFailAlert.hide();

let saveUserProfileButton = $('.js-user-profile-save');

saveUserProfileButton.on('click', () => {
    if (!form.valid()) {
        return;
    }
    let userFirstName = $('.js-firstName').val();
    let userLastName = $('.js-lastName').val();
    let userAddress = $('.js-address').val();
    let userEmail = $('.js-email').val();
    let userId = $('.js-user-id').val();
   
    userSuccessAlert.hide();
    userFailAlert.hide();    

    let userData = {
        "Email": userEmail,
        "FirstName": userFirstName,
        "LastName": userLastName,
        "Address": userAddress
    };   

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/edit/${userId}`,
        contentType: 'application/json',
        data: JSON.stringify(userData)
    }).done(data => {       
        userSuccessAlert.show().delay(3000);
        userSuccessAlert.fadeOut();
    }).fail(failureResponse => {
        userFailAlert.text(failureResponse.responseText);
        userFailAlert.show().delay(3000);
        userFailAlert.fadeOut();
    });
});

//-------------Create Project Form------------------------//

let createProjectSuccessAlert = $('.js-create-project-success-alert');
createProjectSuccessAlert.hide();

let createProjectFailAlert = $('.js-create-project-fail-alert');
createProjectFailAlert.hide();

let createProjectButton = $('.js-create-project-button');

createProjectButton.on('click', () => {
    if (!form.valid()) {
        return;
    }
    let createProjectTitle = $('.js-create-project-title').val();
    let createProjectDescription = $('.js-create-project-description').val();
    let createProjectMainImage = $('.js-create-project-image').val();
    let createProjectDueTo = $('.js-create-project-dueto').val();
    let createProjectGoal = $('.js-create-project-goal').val();
    let createProjectCategory = $('.js-create-project-category').val();
    
    createProjectSuccessAlert.hide();
    createProjectFailAlert.hide();

    let projectData = {
        "Title": createProjectTitle,
        "Description": createProjectDescription,
        "MainImageUrl": createProjectMainImage,
        "DueTo": createProjectDueTo ? createProjectDueTo : null,
        "Goal": parseInt(createProjectGoal),
        "CategoryId": parseInt(createProjectCategory)
    };

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/project/create`,
        contentType: 'application/json',
        data: JSON.stringify(projectData)
    }).done(data => {
        document.querySelector('.js-create-project-form').reset();
        createProjectSuccessAlert.show().delay(3000);
        createProjectSuccessAlert.fadeOut();
    }).fail(failureResponse => {
        createProjectFailAlert.text(failureResponse.responseText);
        createProjectFailAlert.show().delay(3000);
        createProjectFailAlert.fadeOut();
    });
});

//--------------Create Post--------//

$('.js-create-post-success-alert').hide();
$('.js-create-post-fail-alert').hide();

$('.js-createpost').on('click', () => {
    if (!form.valid()) {
        return;
    }
    let title = $('.js-title').val();
    let projectId = $('.js-project').val();
    let description = $('.js-description').val();
    
    let data = {
        "Title": title,
        "ProjectId": parseInt(projectId),
        "Text": description
    };

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/post/project/${projectId}`,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        document.querySelector('.js-create-post-form').reset();
        $('.js-create-post-success-alert').show().delay(3000);
        $('.js-create-post-success-alert').fadeOut();
    }).fail(failureResponse => {
        $('.js-create-post-fail-alert').text(failureResponse.responseText);
        $('.js-create-post-fail-alert').show().delay(3000);
        $('.js-create-post-fail-alert').fadeOut();
    });
});

//-------Create Reward Package----------//

$('.js-create-reward-success-alert').hide();
$('.js-create-reward-fail-alert').hide();

$('.js-createrewardpackage').on('click', () => {
    if (!form.valid()) {
        return;
    }
    let title = $('.js-title').val();
    let amount = $('.js-amount').val();
    let quantity = $('.js-quantity').val();
    let description = $('.js-description').val();
    let projectId = $('.js-projectId').val();

    let data = {
        "ProjectId": parseInt(projectId),
        "Title" : title,    
        "Description": description,
        "MinAmount": amount ? parseInt(amount) : null,
        "Quantity" : quantity ? parseInt(quantity) : null
    };

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/reward/project/${projectId}`,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        document.querySelector('.js-create-reward-form').reset();
        $('.js-create-reward-success-alert').show().delay(3000);
        $('.js-create-reward-success-alert').fadeOut();
    }).fail(failureResponse => {
        $('.js-create-reward-fail-alert').text(failureResponse.responseText);
        $('.js-create-reward-fail-alert').show().delay(3000);
        $('.js-create-reward-fail-alert').fadeOut();
    });
});


//--------------------Project Update/Edit JS-------------------------//

let projectUpdateSuccessAlert = $('.js-project-update-success-alert');
projectUpdateSuccessAlert.hide();

let projectUpdateFailAlert = $('.js-project-update-fail-alert');
projectUpdateFailAlert.hide();

let saveProjectUpdateButton = $('.js-project-update-save');

saveProjectUpdateButton.on('click', () => {
    if (!form.valid()) {
        return;
    }
    let projectTitle = $('.js-title').val();
    let projectDescription = $('.js-description').val();
    let projectMainImageUrl = $('.js-mainImageUrl').val();
    let projectDueTo = $('.js-dueTo').val();
    let projectGoal = $('.js-goal').val();
    let projectCategory = $('.js-project-update-category').val();
    let projectId = $('.js-project-id').val();

    projectUpdateSuccessAlert.hide();
    projectUpdateFailAlert.hide();

    let projectData = {
        "ProjectId": parseInt(projectId),
        "Title": projectTitle,
        "Description": projectDescription,
        "MainImageUrl": projectMainImageUrl,
        "DueTo": projectDueTo,
        "Goal": parseInt(projectGoal),
        "CategoryId": parseInt(projectCategory)
    };

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/project/edit/${projectId}`,
        contentType: 'application/json',
        data: JSON.stringify(projectData)
    }).done(data => {
        projectUpdateSuccessAlert.show().delay(3000);
        projectUpdateSuccessAlert.fadeOut();
    }).fail(failureResponse => {
        projectUpdateFailAlert.text(failureResponse.responseText);
        projectUpdateFailAlert.show().delay(3000);
        projectUpdateFailAlert.fadeOut();
    });
});
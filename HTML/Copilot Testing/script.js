// ========== HAMBURGER MENU TOGGLE ==========
const hamburgerBtn = document.getElementById('hamburger-btn');
const navMenu = document.getElementById('nav-menu');
const navLinks = navMenu.querySelectorAll('a');

/**
 * Toggles the navigation menu visibility on mobile
 */
function toggleMenu() {
    hamburgerBtn.classList.toggle('active');
    navMenu.classList.toggle('active');
    
    // Update aria-expanded for accessibility
    const isExpanded = hamburgerBtn.classList.contains('active');
    hamburgerBtn.setAttribute('aria-expanded', isExpanded);
}

// Add click event listener to hamburger button
hamburgerBtn.addEventListener('click', toggleMenu);

// Close menu when a navigation link is clicked
navLinks.forEach(link => {
    link.addEventListener('click', () => {
        hamburgerBtn.classList.remove('active');
        navMenu.classList.remove('active');
        hamburgerBtn.setAttribute('aria-expanded', 'false');
    });
});

// ========== SMOOTH SCROLLING ==========
/**
 * Enables smooth scrolling behavior for anchor links
 */
function enableSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            
            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);
            
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Initialize smooth scrolling on page load
document.addEventListener('DOMContentLoaded', enableSmoothScroll);

// ========== PROJECTS FILTER ==========
const filterButtons = document.querySelectorAll('.filter-btn');
const projectCards = document.querySelectorAll('.project-card');

/**
 * Filters projects by category
 * @param {string} category - The category to filter by (e.g., 'all', 'web', 'data')
 */
function filterProjects(category) {
    projectCards.forEach(card => {
        if (category === 'all' || card.dataset.category === category) {
            card.classList.remove('hidden');
        } else {
            card.classList.add('hidden');
        }
    });
}

// Add click event listeners to filter buttons
filterButtons.forEach(button => {
    button.addEventListener('click', () => {
        // Remove active class from all buttons
        filterButtons.forEach(btn => btn.classList.remove('active'));
        
        // Add active class to clicked button
        button.classList.add('active');
        
        // Filter projects
        const category = button.dataset.filter;
        filterProjects(category);
    });
});

// ========== LIGHTBOX MODAL ==========
const lightbox = document.getElementById('lightbox');
const lightboxImg = document.getElementById('lightbox-img');
const lightboxCaption = document.getElementById('lightbox-caption');
const lightboxClose = document.querySelector('.lightbox-close');
const lightboxPrev = document.querySelector('.lightbox-prev');
const lightboxNext = document.querySelector('.lightbox-next');

let currentImageIndex = 0;
let visibleImages = [];

/**
 * Opens the lightbox modal with the clicked image
 * @param {HTMLElement} imgElement - The image element that was clicked
 */
function openLightbox(imgElement) {
    // Get all visible (not hidden) project images
    visibleImages = Array.from(document.querySelectorAll('.project-card:not(.hidden) .project-img'));
    
    // Find the index of the clicked image
    currentImageIndex = visibleImages.indexOf(imgElement);
    
    // Display the image in lightbox
    displayLightboxImage();
    
    // Show the lightbox
    lightbox.classList.add('active');
    lightbox.setAttribute('aria-hidden', 'false');
    document.body.style.overflow = 'hidden'; // Prevent scrolling
}

/**
 * Closes the lightbox modal
 */
function closeLightbox() {
    lightbox.classList.remove('active');
    lightbox.setAttribute('aria-hidden', 'true');
    document.body.style.overflow = 'auto'; // Allow scrolling
}

/**
 * Displays the current image in the lightbox
 */
function displayLightboxImage() {
    if (visibleImages.length === 0) return;
    
    const imgElement = visibleImages[currentImageIndex];
    lightboxImg.src = imgElement.src;
    lightboxImg.alt = imgElement.alt;
    lightboxCaption.textContent = imgElement.closest('.project-card').querySelector('figcaption').textContent;
}

/**
 * Shows the next image in the lightbox
 */
function nextImage() {
    currentImageIndex = (currentImageIndex + 1) % visibleImages.length;
    displayLightboxImage();
}

/**
 * Shows the previous image in the lightbox
 */
function prevImage() {
    currentImageIndex = (currentImageIndex - 1 + visibleImages.length) % visibleImages.length;
    displayLightboxImage();
}

// Add click event listeners to all project images
document.querySelectorAll('.project-img').forEach(img => {
    img.addEventListener('click', () => openLightbox(img));
});

// Close lightbox when close button is clicked
lightboxClose.addEventListener('click', closeLightbox);

// Navigate to next/prev images
lightboxNext.addEventListener('click', nextImage);
lightboxPrev.addEventListener('click', prevImage);

// Close lightbox when clicking outside the image
lightbox.addEventListener('click', (e) => {
    if (e.target === lightbox) {
        closeLightbox();
    }
});

// Keyboard navigation for lightbox
document.addEventListener('keydown', (e) => {
    if (!lightbox.classList.contains('active')) return;
    
    if (e.key === 'Escape') {
        closeLightbox();
    } else if (e.key === 'ArrowRight') {
        nextImage();
    } else if (e.key === 'ArrowLeft') {
        prevImage();
    }
});

// ========== FORM VALIDATION ==========
const contactForm = document.getElementById('contact-form');
const nameField = document.getElementById('name');
const emailField = document.getElementById('email');
const messageField = document.getElementById('message');

/**
 * Validates the name field
 * @returns {boolean} True if valid, false otherwise
 */
function validateName() {
    const name = nameField.value.trim();
    const nameError = document.getElementById('name-error');
    const formGroup = nameField.closest('.form-group');
    
    if (name.length === 0) {
        nameError.textContent = 'Name is required';
        nameError.classList.add('show');
        formGroup.classList.add('error');
        formGroup.classList.remove('success');
        return false;
    } else if (name.length < 2) {
        nameError.textContent = 'Name must be at least 2 characters';
        nameError.classList.add('show');
        formGroup.classList.add('error');
        formGroup.classList.remove('success');
        return false;
    } else {
        nameError.textContent = '';
        nameError.classList.remove('show');
        formGroup.classList.remove('error');
        formGroup.classList.add('success');
        return true;
    }
}

/**
 * Validates the email field
 * @returns {boolean} True if valid, false otherwise
 */
function validateEmail() {
    const email = emailField.value.trim();
    const emailError = document.getElementById('email-error');
    const formGroup = emailField.closest('.form-group');
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    
    if (email.length === 0) {
        emailError.textContent = 'Email is required';
        emailError.classList.add('show');
        formGroup.classList.add('error');
        formGroup.classList.remove('success');
        return false;
    } else if (!emailRegex.test(email)) {
        emailError.textContent = 'Please enter a valid email address';
        emailError.classList.add('show');
        formGroup.classList.add('error');
        formGroup.classList.remove('success');
        return false;
    } else {
        emailError.textContent = '';
        emailError.classList.remove('show');
        formGroup.classList.remove('error');
        formGroup.classList.add('success');
        return true;
    }
}

/**
 * Validates the message field
 * @returns {boolean} True if valid, false otherwise
 */
function validateMessage() {
    const message = messageField.value.trim();
    const messageError = document.getElementById('message-error');
    const formGroup = messageField.closest('.form-group');
    
    if (message.length === 0) {
        messageError.textContent = 'Message is required';
        messageError.classList.add('show');
        formGroup.classList.add('error');
        formGroup.classList.remove('success');
        return false;
    } else if (message.length < 10) {
        messageError.textContent = 'Message must be at least 10 characters';
        messageError.classList.add('show');
        formGroup.classList.add('error');
        formGroup.classList.remove('success');
        return false;
    } else {
        messageError.textContent = '';
        messageError.classList.remove('show');
        formGroup.classList.remove('error');
        formGroup.classList.add('success');
        return true;
    }
}

/**
 * Validates all form fields
 * @returns {boolean} True if all fields are valid, false otherwise
 */
function validateForm() {
    return validateName() && validateEmail() && validateMessage();
}

// Add real-time validation listeners
nameField.addEventListener('blur', validateName);
nameField.addEventListener('input', validateName);

emailField.addEventListener('blur', validateEmail);
emailField.addEventListener('input', validateEmail);

messageField.addEventListener('blur', validateMessage);
messageField.addEventListener('input', validateMessage);

// Handle form submission
contactForm.addEventListener('submit', (e) => {
    e.preventDefault();
    
    if (validateForm()) {
        // Form is valid - show success message
        alert('Thank you! Your message has been sent successfully.');
        // Reset form
        contactForm.reset();
        // Clear validation states
        document.querySelectorAll('.form-group').forEach(group => {
            group.classList.remove('error', 'success');
        });
        document.querySelectorAll('.error-message').forEach(msg => {
            msg.classList.remove('show');
            msg.textContent = '';
        });
    } else {
        // Form is invalid - alert user
        alert('Please fix the errors in the form before submitting.');
    }
});


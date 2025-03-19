function copyLink() {
	var copyText = window.location.href;
	navigator.clipboard.writeText(copyText);

	var button = document.getElementById("shareButton");
	var textSpan = button.querySelector(".text");

	function resetAnimation() {
		textSpan.classList.remove("slide-up", "slide-down");
		void textSpan.offsetWidth; // Trigger reflow
	}

	resetAnimation();
	textSpan.classList.add("slide-up");
	setTimeout(function () {
		textSpan.innerHTML = "Copied";
		resetAnimation();
		textSpan.classList.add("slide-down");
	}, 500);

	setTimeout(function () {
		resetAnimation();
		textSpan.classList.add("slide-up");
		setTimeout(function () {
			textSpan.innerHTML = "Share";
			resetAnimation();
			textSpan.classList.add("slide-down");
		}, 500);
	}, 2000);
}

function copyImage() {
	//src from .snapshot-img
	var img = document.querySelector(".snapshot-img");
	var src = img.src;
	var copyText = src;
	navigator.clipboard.writeText(copyText);

	var button = document.getElementById("copyButton");
	var textSpan = button.querySelector(".text");

	function resetAnimation() {
		textSpan.classList.remove("slide-up", "slide-down");
		void textSpan.offsetWidth; // Trigger reflow
	}

	resetAnimation();
	textSpan.classList.add("slide-up");
	setTimeout(function () {
		textSpan.innerHTML = "Copied";
		resetAnimation();
		textSpan.classList.add("slide-down");
	}, 500);

	setTimeout(function () {
		resetAnimation();
		textSpan.classList.add("slide-up");
		setTimeout(function () {
			textSpan.innerHTML = "Copy Image";
			resetAnimation();
			textSpan.classList.add("slide-down");
		}, 500);
	}, 2000);
}
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
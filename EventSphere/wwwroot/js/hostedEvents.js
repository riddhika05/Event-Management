document.addEventListener("DOMContentLoaded", async function () {
    const container = document.getElementById("hostedEventsContainer");
    const token = localStorage.getItem("token");

    if (!token) {
        container.innerHTML = `<p class="text-danger">You must be logged in to view hosted events.</p>`;
        return;
    }

    try {
        const response = await fetch("/Events/Get_Hosted_Event", {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            container.innerHTML = `<p class="text-danger">Failed to fetch hosted events.</p>`;
            return;
        }

        const events = await response.json();

        if (events.length === 0) {
            container.innerHTML = `<p class="text-muted">You haven't hosted any events yet.</p>`;
            return;
        }

        for (const ev of events) {
            const card = document.createElement("div");
            card.className = "col-md-4";

            card.innerHTML = `
                <div class="card shadow-sm h-100">
                    <img src="${ev.imageUrl}" class="card-img-top" alt="${ev.name}" style="height: 200px; object-fit: cover;">
                    <div class="card-body">
                        <h5 class="card-title">${ev.name}</h5>
                        <p><strong>Date:</strong> ${new Date(ev.date).toLocaleString()}</p>
                        <p><strong>Location:</strong> ${ev.location}</p>
                        <p><strong>Attendees:</strong> ${ev.attendeeCount}</p>
                        <a href="/Events/Describe?eventId=${ev.id}" class="btn btn-primary w-100">View Details</a>
                    </div>
                </div>
            `;

            container.appendChild(card);
        }
    } catch (err) {
        console.error(err);
        container.innerHTML = `<p class="text-danger">An error occurred. Please try again.</p>`;
    }
});

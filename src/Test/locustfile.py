from locust import HttpUser, task, between
import json

class WebsiteUser(HttpUser):
    #wait_time = between(0, 0.00167)  # Roughly equivalent to 600 requests per second

    def on_start(self):
        with open('generated.json', 'r') as f:
            self.payload = json.load(f)

    @task
    def send_payload(self):
        self.client.post('', json=self.payload)

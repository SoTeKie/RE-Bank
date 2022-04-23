from django.urls import include, path
from rest_framework import routers
from rest_framework.authtoken import views
from .views import register


from .views import current_user

router = routers.DefaultRouter()

# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
    path("", include(router.urls)),
    path("me/", current_user),
    path("login/", views.obtain_auth_token),
    path("register/", register),
]

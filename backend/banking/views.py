from dataclasses import dataclass

from django.contrib.auth.models import User
from rest_framework.decorators import api_view, permission_classes
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from .models import UserProfile


@api_view(["GET"])
@permission_classes((IsAuthenticated,))
def current_user(request):
    user = request.user
    profile = user.userprofile
    return Response(profile.full_profile)


@api_view(["POST"])
def register(request):
    @dataclass()
    class RegistrationRequest:
        email: str
        password: str
        phone: str
        title: str
        first_name: str
        middle_name: str
        last_name: str

    if not request.user.is_anonymous:
        return Response(status=403)

    req = RegistrationRequest(**request.data)
    user = User.objects.create_user(
        req.email,
        email=req.email,
        password=req.password,
        first_name=req.first_name,
        last_name=req.last_name,
    )
    profile = UserProfile.objects.create(
        phone_number=req.phone,
        middle_name=req.middle_name,
        title=req.title,
        user_id=user.id,
    )
    profile.save()
    return Response(profile.full_profile)

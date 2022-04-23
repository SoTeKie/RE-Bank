import datetime
from dataclasses import dataclass

from django.contrib.auth.models import User
from django.contrib.auth.password_validation import validate_password
from django.core.validators import validate_email
from django.db import transaction
from rest_framework import viewsets
from rest_framework.decorators import api_view, permission_classes
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from . import serializers
from . import models


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
        date_of_birth: str

        def __post_init__(self):
            validate_email(self.email)
            validate_password(self.password)

    if not request.user.is_anonymous:
        return Response(status=403)
    req = RegistrationRequest(**request.data)

    with transaction.atomic():
        user = User.objects.create_user(
            req.email,
            email=req.email,
            password=req.password,
            first_name=req.first_name,
            last_name=req.last_name,
        )
        profile = models.UserProfile.objects.create(
            phone_number=req.phone,
            middle_name=req.middle_name,
            title=req.title,
            user_id=user.id,
            date_of_birth=datetime.datetime.strptime(
                req.date_of_birth, "%Y.%m.%d"
            ).date(),
        )
        profile.save()
    return Response(profile.full_profile)


class CardView(viewsets.ModelViewSet):
    serializer_class = serializers.CardSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return models.Card.objects.filter(
            bank_account__user=self.request.user.userprofile
        )


class BankAccountView(viewsets.ModelViewSet):
    serializer_class = serializers.BankAccountSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return models.BankAccount.objects.filter(user=self.request.user.userprofile)


class TransactionView(viewsets.ModelViewSet):
    serializer_class = serializers.TransactionSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return models.Transaction.objects.filter(
            bank_account__user=self.request.user.userprofile
        )


class BillingAddressView(viewsets.ModelViewSet):
    serializer_class = serializers.BillingAddressSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return models.BillingAddress.objects.filter(user=self.request.user.userprofile)


class ShippingAddressView(viewsets.ModelViewSet):
    serializer_class = serializers.ShippingAddressSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return models.ShippingAddress.objects.filter(user=self.request.user.userprofile)

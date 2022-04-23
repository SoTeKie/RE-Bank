from dataclasses import dataclass

from django.contrib.auth.models import User
from django.contrib.auth.password_validation import validate_password
from django.core.validators import validate_email
from rest_framework import viewsets
from rest_framework.decorators import api_view, permission_classes
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from .models import (
    UserProfile,
    Card,
    BankAccount,
    Transaction,
    BillingAddress,
    ShippingAddress,
)
from .serializers import (
    CardSerializer,
    BankAccountSerializer,
    TransactionSerializer,
    BillingAddressSerializer,
    ShippingAddressSerializer,
)


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

        def __post_init__(self):
            validate_email(self.email)
            validate_password(self.password)

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


class CardView(viewsets.ModelViewSet):
    serializer_class = CardSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return Card.objects.filter(bank_account__user=self.request.user.userprofile)


class BankAccountView(viewsets.ModelViewSet):
    serializer_class = BankAccountSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return BankAccount.objects.filter(user=self.request.user.userprofile)


class TransactionView(viewsets.ModelViewSet):
    serializer_class = TransactionSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return Transaction.objects.filter(
            bank_account__user=self.request.user.userprofile
        )


class BillingAddressView(viewsets.ModelViewSet):
    serializer_class = BillingAddressSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return BillingAddress.objects.filter(user=self.request.user.userprofile)


class ShippingAddressView(viewsets.ModelViewSet):
    serializer_class = ShippingAddressSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return ShippingAddress.objects.filter(user=self.request.user.userprofile)

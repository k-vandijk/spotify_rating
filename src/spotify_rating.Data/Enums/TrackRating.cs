namespace spotify_rating.Data.Enums;

public enum TrackRating
{
    LIKE,
    SUPER_LIKE,
    DISLIKE,
}

public static class TrackRatingHelper
{
    public static bool TryConvertToRating(int value, out TrackRating rating)
    {
        switch (value)
        {
            case 0:
                rating = TrackRating.LIKE;
                return true;
            case 1:
                rating = TrackRating.SUPER_LIKE;
                return true;
            case 2:
                rating = TrackRating.DISLIKE;
                return true;
            default:
                rating = default;
                return false;
        }
    }
}